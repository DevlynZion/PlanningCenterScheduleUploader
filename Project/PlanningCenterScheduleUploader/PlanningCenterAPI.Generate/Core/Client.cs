using PlanningCenterAPI.Helper;
using System.Net.Http.Headers;
using System.Text;

namespace PlanningCenterAPI.Generate.Core
{
	public class Client : IDisposable
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

			httpClient = new HttpClient(handler);
			httpClient.BaseAddress = new Uri(BaseAddress);
			httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationstring);
		}

		public async Task<string> Get(string endpoint)
		{
			var response = await httpClient.GetAsync(endpoint);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadAsStringAsync();
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
