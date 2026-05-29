using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using PlanningCenterAPI.Helper;

namespace PlanningCenterAPI.Core
{
    /// <summary>
    /// Manages the HTTPS calls to the Planning Center API.
    /// Supports both Personal Access Token (Basic) and OAuth 2.0 (Bearer) authentication.
    /// </summary>
    internal class Client : IDisposable
    {
        private const string PublicBaseAddress = "https://api.planningcenteronline.com";

        internal bool CaptureEndPoint { get; set; }
        internal bool CaptureRequest { get; set; }
        internal bool CaptureRespone { get; set; }

        private readonly HttpClient httpClient;

        /// <summary>
        /// Creates a Client using Personal Access Token (Basic) authentication.
        /// This is the original authentication method.
        /// </summary>
        internal Client()
        {
            var authString = Convert.ToBase64String(
                Encoding.ASCII.GetBytes(AuthenticationHelper.GetCredentials()));

            var handler = new SocketsHttpHandler()
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            };

            httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(PublicBaseAddress)
            };
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", authString);
        }

        /// <summary>
        /// Private constructor for OAuth 2.0 Bearer token authentication.
        /// Uses a delegating handler to inject a fresh Bearer token on each request.
        /// </summary>
        private Client(Func<Task<string?>> tokenProvider)
        {
            var handler = new SocketsHttpHandler()
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(2),
            };

            httpClient = new HttpClient(new TokenInjectionHandler(tokenProvider, handler))
            {
                BaseAddress = new Uri(PublicBaseAddress)
            };
        }

        /// <summary>
        /// Factory method to create a Client using OAuth 2.0 Bearer token authentication.
        /// </summary>
        internal static Client CreateWithOAuth(Func<Task<string?>> tokenProvider) =>
            new Client(tokenProvider);

        public async Task<T> Get<T>(string endpoint)
        {
            WriteEndPoint(endpoint);

            var response = await httpClient.GetAsync(endpoint);
            await WrtieResponse(response);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<T>(
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = false });
            return data;
        }

        public async Task<T> Post<T, C>(string endpoint, C content)
        {
            WriteEndPoint(endpoint);

            var response = await httpClient.PostAsJsonAsync(endpoint, content);
            await WrtieResponse(response);
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadFromJsonAsync<T>(
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = false });
            return data;
        }

        public async Task Delete(string endpoint)
        {
            WriteEndPoint(endpoint);

            var response = await httpClient.DeleteAsync(endpoint);
            await WrtieResponse(response);
            response.EnsureSuccessStatusCode();
        }

        private void WriteEndPoint(string endpoint)
        {
            if (CaptureEndPoint)
                File.WriteAllText("EndPoint.txt", PublicBaseAddress + endpoint);
        }

        private async Task WrtieResponse(HttpResponseMessage response)
        {
            if (CaptureRespone)
            {
                var stringResponse = await response.Content.ReadAsStringAsync();
                File.WriteAllText("Respone.txt", stringResponse);
            }
        }

        public void Dispose()
        {
            httpClient.Dispose();
        }

        /// <summary>
        /// HTTP message handler that injects a fresh Bearer token on each request.
        /// </summary>
        private sealed class TokenInjectionHandler : DelegatingHandler
        {
            private readonly Func<Task<string?>> _tokenProvider;

            public TokenInjectionHandler(Func<Task<string?>> tokenProvider, HttpMessageHandler innerHandler)
                : base(innerHandler)
            {
                _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
            }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var token = await _tokenProvider();
                if (!string.IsNullOrEmpty(token))
                {
                    request.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", token);
                }

                return await base.SendAsync(request, cancellationToken);
            }
        }
    }
}
