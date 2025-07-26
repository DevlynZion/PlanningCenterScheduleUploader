using PlanningCenterAPI.Helper;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace PlanningCenterAPI.Core
{
	/// <summary>
	/// Manages the Https calls
	/// </summary>
	internal class Client : IDisposable
	{
		private const string BaseAddress = "https://api.planningcenteronline.com";

		private HttpClient httpClient;
		private bool disposedValue;

		internal Client()
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

		public async Task<T> Get<T>(string endpoint)
		{
			var response = await httpClient.GetAsync(endpoint);
			response.EnsureSuccessStatusCode();

			var a = await response.Content.ReadAsStringAsync();
			File.WriteAllText("Respone.txt", a.ToString());

			var data = await response.Content.ReadFromJsonAsync<T>();
			return data;
		}

		public async Task Post<T>(string endpoint, T data)
		{
			var response = await httpClient.PostAsJsonAsync<T>(endpoint, data);

			response.EnsureSuccessStatusCode();
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
