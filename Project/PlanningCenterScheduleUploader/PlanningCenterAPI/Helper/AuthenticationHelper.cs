using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace PlanningCenterAPI.Helper
{
    [SupportedOSPlatform("windows")]
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

            var encryptedSecret = ProtectedData.Protect(
                Encoding.UTF8.GetBytes(secret),
                null,
                DataProtectionScope.CurrentUser);

            var stored = new StoredCredentials
            {
                AppId = appId,
                EncryptedSecret = Convert.ToBase64String(encryptedSecret)
            };

            File.WriteAllText(CredentialsPath, JsonSerializer.Serialize(stored));
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
                var stored = JsonSerializer.Deserialize<StoredCredentials>(json);
                if (stored is null) return null;

                string secret;

                if (stored.EncryptedSecret is not null)
                {
                    var encrypted = Convert.FromBase64String(stored.EncryptedSecret);
                    secret = Encoding.UTF8.GetString(
                        ProtectedData.Unprotect(encrypted, null, DataProtectionScope.CurrentUser));
                }
                else if (stored.Secret is not null)
                {
                    // Legacy plaintext file — migrate to encrypted format immediately
                    secret = stored.Secret;
                    SaveCredentials(stored.AppId, secret);
                }
                else
                {
                    return null;
                }

                return new Credentials { AppId = stored.AppId, Secret = secret };
            }
            catch
            {
                return null;
            }
        }

        // Disk format — Secret is nullable for legacy migration only
        private class StoredCredentials
        {
            public string AppId { get; set; } = string.Empty;
            public string? Secret { get; set; }
            public string? EncryptedSecret { get; set; }
        }

        private class Credentials
        {
            public string AppId { get; set; } = string.Empty;
            public string Secret { get; set; } = string.Empty;
        }
    }
}
