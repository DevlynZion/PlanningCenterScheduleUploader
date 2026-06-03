using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PlanningCenterScheduleUploader
{
    internal static class UpdateChecker
    {
        private const string LatestReleaseUrl =
            "https://api.github.com/repos/DevlynZion/PlanningCenterScheduleUploader/releases/latest";

        internal record UpdateResult(bool HasUpdate, string LatestTag, string ReleaseUrl);

        internal static async Task<UpdateResult> CheckForUpdateAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("PlanningCenterScheduleUploader");

                var release = await client.GetFromJsonAsync<GitHubRelease>(LatestReleaseUrl);

                if (release is null || string.IsNullOrWhiteSpace(release.TagName))
                    return new UpdateResult(false, string.Empty, string.Empty);

                var currentVersion = GetCurrentVersion();
                var hasUpdate = !string.Equals(release.TagName, currentVersion, StringComparison.OrdinalIgnoreCase);

                return new UpdateResult(hasUpdate, release.TagName, release.HtmlUrl ?? string.Empty);
            }
            catch
            {
                // Never crash the app over a failed update check
                return new UpdateResult(false, string.Empty, string.Empty);
            }
        }

        private static string GetCurrentVersion()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            return version is null ? string.Empty : $"v{version.Major}.{version.Minor}.{version.Build}";
        }

        private class GitHubRelease
        {
            [JsonPropertyName("tag_name")]
            public string? TagName { get; set; }

            [JsonPropertyName("html_url")]
            public string? HtmlUrl { get; set; }
        }
    }
}
