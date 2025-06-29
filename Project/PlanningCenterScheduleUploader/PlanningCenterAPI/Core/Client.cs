using PlanningCenterAPI.Helper;
using System.Net.Http.Headers;
using System.Text;

namespace PlanningCenterAPI.Core
{
	internal class Client : IDisposable
	{
		private const string BaseAddress = "https://api.planningcenteronline.com";

		private HttpClient httpClient;
		private bool disposedValue;

		public Client()
		{
			var authenticationstring = Convert.ToBase64String(Encoding.ASCII.GetBytes(AuthenticationHelper.GetCredentials()));

			var handler = new SocketsHttpHandler()
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(2),
			};

			HttpClient client = new HttpClient(handler);

			client.BaseAddress = new Uri(BaseAddress);
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationstring);
		}

		public void CreateRequest<T>()
		{

		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					httpClient.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}
	}
}
