namespace PlanningCenterAPI.OAuth
{
    /// <summary>
    /// Provides Bearer token management for authenticated API requests.
    /// Automatically refreshes tokens before they expire.
    /// </summary>
    public sealed class OAuthAuthenticator
    {
        private readonly OAuthTokenManager _tokenManager;
        private readonly string _baseAddress;

        public OAuthAuthenticator(OAuthConfig config)
        {
            _tokenManager = new OAuthTokenManager(config);
            _baseAddress = config.BaseAddress;
        }

        /// <summary>
        /// Initializes the authenticator: loads persisted tokens.
        /// Call this after the user completes the OAuthLogin flow.
        /// </summary>
        public async Task InitializeAsync(CancellationToken ct = default)
        {
            await _tokenManager.GetAccessTokenAsync(ct);
        }

        /// <summary>
        /// Gets a valid (refreshed if necessary) Bearer access token, or null if not authenticated.
        /// </summary>
        public async Task<string?> GetBearerTokenAsync(CancellationToken ct = default)
        {
            return await _tokenManager.GetAccessTokenAsync(ct);
        }

        /// <summary>
        /// Returns a Func that can be injected into the Client to provide the Bearer header per-request.
        /// </summary>
        public Func<Task<string?>> GetTokenProvider() =>
            async () => await GetBearerTokenAsync();

        /// <summary>
        /// Clears all stored tokens. Call when the user wants to log out or switch accounts.
        /// </summary>
        public void Logout()
        {
            _tokenManager.ClearTokens();
        }

        /// <summary>
        /// Checks if valid tokens exist (loaded from disk or memory).
        /// </summary>
        public async Task<bool> IsAuthenticatedAsync(CancellationToken ct = default)
        {
            var token = await _tokenManager.GetAccessTokenAsync(ct);
            return !string.IsNullOrEmpty(token);
        }

        /// <summary>
        /// Revokes the current access token on Planning Center's side and clears local storage.
        /// Best-effort — does not throw.
        /// </summary>
        public async Task RevokeAsync(string clientId, string clientSecret, CancellationToken ct = default)
        {
            var token = await _tokenManager.GetAccessTokenAsync(ct);
            if (token is not null)
            {
                try
                {
                    using var client = new HttpClient();
                    var requestContent = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        ["token"] = token,
                        ["token_type_hint"] = "access_token",
                        ["client_id"] = clientId,
                        ["client_secret"] = clientSecret
                    });
                    await client.PostAsync($"{_baseAddress}/oauth/revoke", requestContent, ct);
                }
                catch { /* best-effort revocation */ }
            }
            _tokenManager.ClearTokens();
        }
    }
}
