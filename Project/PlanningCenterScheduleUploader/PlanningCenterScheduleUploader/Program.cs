using System.Net.Http.Json;
using System.Net.Http;
using System;
using System.Text;
using System.Net.Security;
using PlanningCenterAPI.Helper;
using PlanningCenterAPI.Call;

namespace PlanningCenterScheduleUploader
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Test3();
			Console.ReadKey();
		}

		static async void Test3()
		{
			var handler = new SocketsHttpHandler()
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(2),
			};

			using (HttpClient client = new HttpClient(handler))
			{
				client.BaseAddress = new Uri("https://api.planningcenteronline.com");
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(AuthenticationHelper.GetCredentials())));

				var request = new Services(client);
				var data = await request.GetPeople();

				Console.WriteLine($"Data returned = {data.Data.Count}");
			}
		}

		static async void Test2()
		{
			var handler = new SocketsHttpHandler()
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(2),
			};

			using (HttpClient client = new HttpClient(handler))
			{
				client.BaseAddress = new Uri("https://api.planningcenteronline.com");
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(AuthenticationHelper.GetCredentials())));
				var response = await client.GetAsync("/services/v2/people");
				response.EnsureSuccessStatusCode();
				var responseData = await response.Content.ReadFromJsonAsync<Root>();
				Console.WriteLine(responseData);
			}
		}

		static async void Test()
		{
			var handler = new SocketsHttpHandler()
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(2),
			};

			using (HttpClient client = new HttpClient(handler))
			{
				client.BaseAddress = new Uri("https://api.planningcenteronline.com");
				client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(AuthenticationHelper.GetCredentials())));
				var response = await client.GetAsync("/people/v2/people");
				var responseString = await response.Content.ReadAsStringAsync();
				Console.WriteLine(responseString);
			}
		}
	}

	public class Root
	{
		public Links Links { get; set; }
		public List<Data> Data { get; set; }

		public override string ToString()
		{
			return $"Data={Data.Count}";
		}
	}


	public class Links
	{
		public string Self { get; set; }
		public string Next { get; set; }
	}

	public class Data
	{
		public string Type { get; set; }
		public string Id { get; set; }
	}
}