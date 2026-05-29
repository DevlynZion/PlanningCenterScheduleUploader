namespace PlanningCenterAPI.OAuth
{
    /// <summary>
    /// Configuration for Planning Center OAuth 2.0 authentication.
    /// </summary>
    public sealed record OAuthConfig
    {
        /// <summary>
        /// The Client ID from your Planning Center OAuth application.
        /// </summary>
        public required string ClientId { get; init; }

        /// <summary>
        /// The Client Secret from your Planning Center OAuth application.
        /// </summary>
        public required string ClientSecret { get; init; }

        /// <summary>
        /// The redirect URI registered with your OAuth application.
        /// For desktop apps, use http://localhost:PORT/callback
        /// </summary>
        public required string RedirectUri { get; init; } = "http://localhost:7168/callback";

        /// <summary>
        /// OAuth scopes to request. Default covers People and Services modules.
        /// </summary>
        public required string Scopes { get; init; } = "people services";

        /// <summary>
        /// Base URL for Planning Center API. Override only for testing.
        /// </summary>
        public string BaseAddress { get; init; } = "https://api.planningcenteronline.com";
    }
}
