using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanningCenterAPI.Helper
{
	public static class AuthenticationHelper
	{
		private const string PathToSecretFile = @"D:\Projects\GitHub\PlanningCenterScheduleUploader\Document\Secret.txt";

		public static string GetCredentials()
		{
			var lines = File.ReadAllLines(PathToSecretFile);

			return $"{lines[1]}:{lines[4]}";
		}
	}
}
