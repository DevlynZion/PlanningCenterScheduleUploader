using System.Text.Json.Serialization;

namespace PlanningCenterAPI.OAuth
{
    /// <summary>
    /// Response from Planning Center's /oauth/token endpoint.
    /// </summary>
    internal sealed record OAuthTokenResponse
    {
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; init; }

        [JsonPropertyName("token_type")]
        public required string TokenType { get; init; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        [JsonPropertyName("refresh_token")]
        public required string RefreshToken { get; init; }

        [JsonPropertyName("scope")]
        public required string Scope { get; init; }

        [JsonPropertyName("created_at")]
        public long CreatedAt { get; init; }
    }
}
