using System.Text.Json;

namespace PlanningCenterAPI.OAuth
{
    /// <summary>
    /// Represents the stored token state that gets persisted to disk.
    /// </summary>
    internal sealed record StoredTokens
    {
        public string AccessToken { get; init; } = string.Empty;
        public string RefreshToken { get; init; } = string.Empty;
        public long CreatedAt { get; init; }
        public int ExpiresIn { get; init; }
        public string Scope { get; init; } = string.Empty;
    }

    /// <summary>
    /// Persists and manages OAuth token lifecycle — loading, saving, expiry checks, and refreshing.
    /// </summary>
    internal sealed class OAuthTokenManager
    {
        private readonly OAuthConfig _config;
        private readonly string _tokenFilePath;
        private readonly HttpClient _httpClient;

        private StoredTokens? _tokens;
        private readonly SemaphoreSlim _refreshLock = new(1, 1);

        /// <summary>
        /// Raised when new tokens are obtained (initial login or refresh).
        /// </summary>
        public event Action<StoredTokens>? TokensUpdated;

        public OAuthTokenManager(OAuthConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _httpClient = new HttpClient();
            _tokenFilePath = GetTokenFilePath();
        }

        /// <summary>
        /// Gets a valid access token, refreshing if necessary.
        /// Returns null if no tokens are stored or refresh fails.
        /// </summary>
        public async Task<string?> GetAccessTokenAsync(CancellationToken ct = default)
        {
            // Load from disk if not in memory
            if (_tokens is null)
                LoadTokens();

            if (_tokens is null)
                return null;

            // Check if the current token is still valid (with 5-minute buffer)
            if (!IsTokenExpired(_tokens))
                return _tokens.AccessToken;

            // Token expired — try to refresh
            return await RefreshTokensAsync(ct);
        }

        /// <summary>
        /// Exchanges an authorization code for tokens (initial login).
        /// </summary>
        public async Task<StoredTokens> ExchangeCodeForTokensAsync(string code, CancellationToken ct = default)
        {
            var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["client_id"] = _config.ClientId,
                ["client_secret"] = _config.ClientSecret,
                ["redirect_uri"] = _config.RedirectUri
            });

            return await RequestTokensAsync(requestContent, ct);
        }

        /// <summary>
        /// Refreshes the current access token using the stored refresh token.
        /// </summary>
        public async Task<string?> RefreshTokensAsync(CancellationToken ct = default)
        {
            await _refreshLock.WaitAsync(ct);
            try
            {
                // Double-check after acquiring lock — another thread may have refreshed
                LoadTokens();
                if (_tokens is not null && !IsTokenExpired(_tokens))
                    return _tokens.AccessToken;

                if (_tokens is null)
                    return null;

                var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "refresh_token",
                    ["refresh_token"] = _tokens.RefreshToken,
                    ["client_id"] = _config.ClientId,
                    ["client_secret"] = _config.ClientSecret
                });

                var newTokens = await RequestTokensAsync(requestContent, ct);
                return newTokens?.AccessToken;
            }
            finally
            {
                _refreshLock.Release();
            }
        }

        /// <summary>
        /// Clears stored tokens (for re-authorization).
        /// </summary>
        public void ClearTokens()
        {
            _tokens = null;
            try
            {
                if (File.Exists(_tokenFilePath))
                    File.Delete(_tokenFilePath);
            }
            catch
            {
                // Best-effort cleanup
            }
        }

        private async Task<StoredTokens?> RequestTokensAsync(FormUrlEncodedContent content, CancellationToken ct)
        {
            var tokenUrl = $"{_config.BaseAddress}/oauth/token";
            var response = await _httpClient.PostAsync(tokenUrl, content, ct);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync(ct);
                throw new InvalidOperationException(
                    $"Token request failed (HTTP {(int)response.StatusCode}): {errorBody}");
            }

            var json = await response.Content.ReadAsStringAsync(ct);
            var tokenResponse = JsonSerializer.Deserialize<OAuthTokenResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (tokenResponse is null)
                throw new InvalidOperationException("Failed to deserialize token response.");

            var stored = new StoredTokens
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                CreatedAt = tokenResponse.CreatedAt,
                ExpiresIn = tokenResponse.ExpiresIn,
                Scope = tokenResponse.Scope
            };

            _tokens = stored;
            SaveTokens(stored);
            TokensUpdated?.Invoke(stored);

            return stored;
        }

        private void SaveTokens(StoredTokens tokens)
        {
            try
            {
                var json = JsonSerializer.Serialize(tokens, new JsonSerializerOptions { WriteIndented = true });
                var directory = Path.GetDirectoryName(_tokenFilePath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                // Write atomically — write to temp then move
                var tempFile = _tokenFilePath + ".tmp";
                File.WriteAllText(tempFile, json);
                File.Move(tempFile, _tokenFilePath, overwrite: true);
            }
            catch
            {
                // Persistence failure shouldn't break auth — tokens still work in memory
            }
        }

        private void LoadTokens()
        {
            try
            {
                if (!File.Exists(_tokenFilePath))
                    return;

                var json = File.ReadAllText(_tokenFilePath);
                _tokens = JsonSerializer.Deserialize<StoredTokens>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                _tokens = null;
            }
        }

        private static bool IsTokenExpired(StoredTokens tokens)
        {
            var epoch = new DateTimeOffset(2001, 1, 1, 0, 0, 0, TimeSpan.Zero);
            var issuedAt = epoch.AddSeconds(tokens.CreatedAt);
            // 5-minute buffer before actual expiry
            return DateTimeOffset.UtcNow > issuedAt.AddSeconds(tokens.ExpiresIn).AddMinutes(-5);
        }

        private static string GetTokenFilePath()
        {
            // Store in user's local app data: %LOCALAPPDATA%\PlanningCenterScheduleUploader\tokens.json
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(appData, "PlanningCenterScheduleUploader", "tokens.json");
        }
    }
}
