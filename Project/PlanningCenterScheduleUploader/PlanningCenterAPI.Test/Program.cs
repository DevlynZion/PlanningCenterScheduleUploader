using System.Net.Http.Json;
using System.Text;
using PlanningCenterAPI;
using PlanningCenterAPI.Helper;

namespace PlanningCenterScheduleUploader
{
	internal class Program
	{
		const int StressIterations = 150;
		const int ProgramWait = 100000;

		static void Main(string[] args)
		{
			TestQueryNeededPosition().Wait();
			//StressTest1().Wait();
			TestPeople();


			Console.WriteLine($"Program Done");
			Console.ReadKey();
		}

		static async Task TestQueryNeededPosition()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				var data = await pco.Services.GetService_types();
			}
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
				Console.WriteLine($"{k} Data returned = {data.data.Count}");
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

		static async void TestPeople()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				var data = await pco.People.GetPeople();

				Console.WriteLine($"Data returned = {data.data.Count}");
			}
		}
	}
}