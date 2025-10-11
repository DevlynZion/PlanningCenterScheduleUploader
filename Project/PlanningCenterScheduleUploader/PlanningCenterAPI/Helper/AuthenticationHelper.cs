namespace PlanningCenterAPI.Helper
{
    public static class AuthenticationHelper
	{
		private const string PathToSecretFile = @".\..\..\..\..\..\..\Document\Secret.txt";

		public static string GetCredentials()
		{
			var lines = File.ReadAllLines(PathToSecretFile);

			return $"{lines[1]}:{lines[4]}";
		}
	}
}
