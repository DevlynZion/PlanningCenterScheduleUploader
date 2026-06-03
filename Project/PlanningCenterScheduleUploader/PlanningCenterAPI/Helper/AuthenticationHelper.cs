using System.Text.Json;

namespace PlanningCenterAPI.Helper
{
    public static class AuthenticationHelper
    {
        private static readonly string CredentialsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "PlanningCenterScheduleUploader",
            "credentials.json");

        public static string GetCredentials()
        {
            if (!File.Exists(CredentialsPath))
                throw new InvalidOperationException("Credentials not configured. Click Settings to enter your Planning Center API credentials.");

            var creds = ReadCredentials();

            if (string.IsNullOrWhiteSpace(creds?.AppId) || string.IsNullOrWhiteSpace(creds?.Secret))
                throw new InvalidOperationException("Credentials not configured. Click Settings to enter your Planning Center API credentials.");

            return $"{creds.AppId}:{creds.Secret}";
        }

        public static void SaveCredentials(string appId, string secret)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(CredentialsPath)!);
            var json = JsonSerializer.Serialize(new Credentials { AppId = appId, Secret = secret });
            File.WriteAllText(CredentialsPath, json);
        }

        public static bool CredentialsExist()
        {
            if (!File.Exists(CredentialsPath))
                return false;

            var creds = ReadCredentials();
            return !string.IsNullOrWhiteSpace(creds?.AppId) && !string.IsNullOrWhiteSpace(creds?.Secret);
        }

        private static Credentials? ReadCredentials()
        {
            try
            {
                var json = File.ReadAllText(CredentialsPath);
                return JsonSerializer.Deserialize<Credentials>(json);
            }
            catch
            {
                return null;
            }
        }

        private class Credentials
        {
            public string AppId { get; set; } = string.Empty;
            public string Secret { get; set; } = string.Empty;
        }
    }
}
