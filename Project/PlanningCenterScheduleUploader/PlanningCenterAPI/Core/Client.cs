using PlanningCenterAPI.Helper;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace PlanningCenterAPI.Core
{
	/// <summary>
	/// Manages the Https calls
	/// </summary>
	internal class Client : IDisposable
	{
		internal bool CaptureEndPoint { get; set; }
		internal bool CaptureRequest{ get; set; }
		internal bool CaptureRespone { get; set; }

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
			WriteEndPoint(endpoint);

			var response = await httpClient.GetAsync(endpoint);

			await WrtieResponse(response);
			response.EnsureSuccessStatusCode();

			var data = await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = false });
			return data;
		}

		public async Task<T> Post<T, C>(string endpoint, C content)
		{
			WriteEndPoint(endpoint);

			var response = await httpClient.PostAsJsonAsync(endpoint, content);

			await WrtieResponse(response);
			response.EnsureSuccessStatusCode();

			var data = await response.Content.ReadFromJsonAsync<T>(new JsonSerializerOptions() { PropertyNameCaseInsensitive = false });
			return data;
		}

		public async Task Delete(string endpoint)
		{
			WriteEndPoint(endpoint);

			var response = await httpClient.DeleteAsync(endpoint);

			await WrtieResponse(response);
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

		private void WriteEndPoint(string endpoint)
		{
			if (CaptureEndPoint)
				File.WriteAllText("EndPoint.txt", BaseAddress + endpoint);
		}

		private async Task WrtieResponse(HttpResponseMessage response)
		{
			if (CaptureRespone)
			{
				var stringResponse = await response.Content.ReadAsStringAsync();
				File.WriteAllText("Respone.txt", stringResponse.ToString());
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
		}
	}
}
