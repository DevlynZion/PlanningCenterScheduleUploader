using System.Net.Http.Json;
using System.Text;
using PlanningCenterAPI;
using PlanningCenterAPI.Helper;
using PlanningCenterAPI.Type;

namespace PlanningCenterScheduleUploader
{
	internal class Program
	{ 
		const int StressIterations = 150;
		const int ProgramWait = 100000;

		static void Main(string[] args)
		{
			//Test4();

			StressTest1().GetAwaiter();

			Console.WriteLine($"Program Done");
			Console.ReadKey();
		}


		static async Task StressTest1()
		{
			// To fail test set RateLimiter.RequestRateLimit higher than 100
			using (PlanningCenter pco = new PlanningCenter())
			{
				for (int i = 0; i < StressIterations; i++)
				{
					StressWork1(i, pco);
				}

				await Task.Delay(ProgramWait);
				Console.WriteLine($"Stress Test Done!");
			}
		}
		static async Task StressWork1(int k, PlanningCenter pco)
		{
			Console.WriteLine($"{k} Submitted");
			try
			{
				var data = await pco.People.GetPeople();
				Console.WriteLine($"{k} Data returned = {data.Data.Count}");
			}
			catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
			{
				Console.WriteLine($"{k} TooManyRequests must handle");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}

		static async void Test4()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				var data = await pco.People.GetPeople();

				Console.WriteLine($"Data returned = {data.Data.Count}");
			}
		}

		/*static async void Test3()
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
		}*/

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
			Console.ReadKey();
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
			Console.ReadKey();
		}
	}
}