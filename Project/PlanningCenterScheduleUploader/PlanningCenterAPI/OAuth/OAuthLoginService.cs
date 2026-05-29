using System.Diagnostics;
using System.Net;
using System.Web;

namespace PlanningCenterAPI.OAuth
{
    /// <summary>
    /// Result of the OAuth login flow.
    /// </summary>
    public sealed record OAuthLoginResult
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
        public string? AccessToken { get; init; }
        public string? RefreshToken { get; init; }

        public static OAuthLoginResult Failed(string error) => new() { Success = false, Error = error };
        public static OAuthLoginResult Ok(string accessToken, string refreshToken) => new()
        {
            Success = true,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    /// <summary>
    /// Starts a local HTTP listener, opens the browser to the Planning Center OAuth page,
    /// waits for the callback, and exchanges the authorization code for tokens.
    /// </summary>
    public sealed class OAuthLoginService
    {
        private readonly OAuthConfig _config;

        public OAuthLoginService(OAuthConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        /// <summary>
        /// Runs the full OAuth login flow asynchronously.
        /// Opens the browser, waits for the callback, exchanges the code for tokens.
        /// </summary>
        /// <param name="ct">Cancellation token — cancels the HTTP listener wait.</param>
        /// <returns>Login result containing tokens or an error message.</returns>
        public async Task<OAuthLoginResult> LoginAsync(CancellationToken ct = default)
        {
            var tokenManager = new OAuthTokenManager(_config);

            // Extract port from redirect URI
            if (!TryExtractPort(_config.RedirectUri, out var port))
                return OAuthLoginResult.Failed($"Invalid redirect URI: {_config.RedirectUri}. Expected http://localhost:PORT/callback");

            // Start local HTTP listener to capture the OAuth callback
            string? authorizationCode = null;
            string? errorMessage = null;
            var listenerTaskCompletion = new TaskCompletionSource<bool>();

            var listener = new HttpListener();
            var prefix = $"http://localhost:{port}/";
            listener.Prefixes.Add(prefix);

            try
            {
                listener.Start();
            }
            catch (HttpListenerException ex)
            {
                return OAuthLoginResult.Failed(
                    $"Failed to start local HTTP listener on {prefix}. " +
                    $"Try running 'netsh http add urlacl url={prefix} user=Everyone' " +
                    $"or use a different port. Error: {ex.Message}");
            }

            // Handle callbacks on a background thread
            var listenerTask = Task.Run(async () =>
            {
                try
                {
                    var context = await listener.GetContextAsync().WaitAsync(ct);
                    var request = context.Request;
                    var response = context.Response;

                    var queryParams = HttpUtility.ParseQueryString(request.Url!.Query);

                    if (queryParams["error"] is not null)
                    {
                        errorMessage = queryParams["error_description"] ?? queryParams["error"];
                    }
                    else if (queryParams["code"] is string code)
                    {
                        authorizationCode = code;
                    }
                    else
                    {
                        errorMessage = "No authorization code or error received in callback.";
                    }

                    // Respond to the browser
                    string responseHtml = errorMessage is not null
                        ? GetErrorHtml(errorMessage)
                        : GetSuccessHtml();

                    var buffer = System.Text.Encoding.UTF8.GetBytes(responseHtml);
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "text/html; charset=utf-8";
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, ct);
                    response.Close();

                    listenerTaskCompletion.TrySetResult(true);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    listenerTaskCompletion.TrySetException(ex);
                }
            }, ct);

            try
            {
                // Open the browser to the Planning Center OAuth authorize page
                var authorizeUrl = BuildAuthorizeUrl();
                OpenBrowser(authorizeUrl);

                // Wait for the callback (or cancellation)
                using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
                cts.CancelAfter(TimeSpan.FromMinutes(5)); // 5-minute timeout for user to complete auth
                await listenerTaskCompletion.Task.WaitAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                return OAuthLoginResult.Failed("Login timed out or was cancelled.");
            }
            finally
            {
                try { listener.Stop(); } catch { /* ignore */ }
                try { listener.Close(); } catch { /* ignore */ }
            }

            if (errorMessage is not null)
                return OAuthLoginResult.Failed(errorMessage);

            if (string.IsNullOrEmpty(authorizationCode))
                return OAuthLoginResult.Failed("No authorization code received.");

            // Exchange the code for tokens
            try
            {
                var stored = await tokenManager.ExchangeCodeForTokensAsync(authorizationCode, ct);
                return OAuthLoginResult.Ok(stored.AccessToken, stored.RefreshToken);
            }
            catch (HttpRequestException ex)
            {
                return OAuthLoginResult.Failed($"Token exchange failed: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                return OAuthLoginResult.Failed(ex.Message);
            }
        }

        private string BuildAuthorizeUrl()
        {
            var scopes = Uri.EscapeDataString(_config.Scopes);
            var redirectUri = Uri.EscapeDataString(_config.RedirectUri);
            var clientId = Uri.EscapeDataString(_config.ClientId);

            return $"{_config.BaseAddress}/oauth/authorize" +
                   $"?client_id={clientId}" +
                   $"&redirect_uri={redirectUri}" +
                   "&response_type=code" +
                   $"&scope={scopes}";
        }

        private static bool TryExtractPort(string redirectUri, out int port)
        {
            port = 0;
            try
            {
                var uri = new Uri(redirectUri);
                if (uri.Host != "localhost" && uri.Host != "127.0.0.1")
 return false;
                port = uri.Port;
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void OpenBrowser(string url)
        {
            try
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            }
            catch
            {
                // Fallback for Linux/WSL
                try
                {
                    Process.Start("xdg-open", url);
                }
                catch
                {
                    Console.WriteLine($"Please open the following URL in your browser: {url}");
                }
            }
        }

        private static string GetSuccessHtml() =>
            """
            <!DOCTYPE html>
            <html>
            <head><title>Authorization Successful</title></head>
            <body style="font-family:sans-serif;text-align:center;padding:60px;background:#f5f5f5;">
                <div style="max-width:400px;margin:auto;background:white;padding:40px;border-radius:12px;box-shadow:0 2px 12px rgba(0,0,0,0.1);">
                    <h1 style="color:#2465F5;">&#10003; Authorization Successful</h1>
                    <p>You have successfully authorized the application.<br>You may now close this window and return to the app.</p>
                </div>
            </body>
            </html>
            """;

        private static string GetErrorHtml(string error) =>
            $"""
            <!DOCTYPE html>
            <html>
            <head><title>Authorization Failed</title></head>
            <body style="font-family:sans-serif;text-align:center;padding:60px;background:#f5f5f5;">
                <div style="max-width:400px;margin:auto;background:white;padding:40px;border-radius:12px;box-shadow:0 2px 12px rgba(0,0,0,0.1);">
                    <h1 style="color:#dc3545;">&#10007; Authorization Failed</h1>
                    <p>{System.Net.WebUtility.HtmlEncode(error)}</p>
                    <p>Please close this window and try again in the app.</p>
                </div>
            </body>
            </html>
            """;
    }
}
