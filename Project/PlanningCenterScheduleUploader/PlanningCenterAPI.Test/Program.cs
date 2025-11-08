using PlanningCenterAPI;
using PlanningCenterAPI.Helper;
using PlanningCenterAPI.Type;
using PlanningCenterAPI.Type.Core.Interface;
using PlanningCenterAPI.Type.Implementation;
using PlanningCenterAPI.Type.Implementation.Attribute;
using System.Net.Http.Json;
using System.Text;

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

			Console.WriteLine($"Program Done");
			Console.ReadKey();
		}

		static async Task TestQueryNeededPosition()
		{
			using (PlanningCenter pco = new PlanningCenter(true, true, true))
			{
				var serviceTypeId = await GetServiceType(pco, "Sunday and Other Services");
				var planTemplateID = await GetPlanTemplate(pco, serviceTypeId, "Sunday Service");
				var teamId = await GetTeam(pco, serviceTypeId, "Live Stream");
				var teamPositionIds = await GetTeamPositions(pco, teamId);
				var peopleIds = await GetPeople(pco, teamId);

			}
		}

		private static async Task<string> GetServiceType(PlanningCenter pco, string find)
		{
			var results = await pco.Services.GetService_types();
			var id = string.Empty;

			Console.WriteLine($"ServiceTypes");
			Console.WriteLine("=============");

			do
			{
				foreach (var result in results.data)
				{
					Console.WriteLine($"{result.id} {result.attributes.Name}");
					if (result.attributes.Name == find)
						id = result.id;
				}
				results = await pco.Services.GetNextRequest<ServicesResponse>(results.links);
			} while (results != null);
			Console.WriteLine();

			return id;
		}

		private static async Task<string> GetPlanTemplate(PlanningCenter pco, string withId, string find)
		{
			var results = await pco.Services.GetPlan_templatesByService_typeId(withId);
			var id = string.Empty;

			Console.WriteLine($"PlanTemplates");
			Console.WriteLine("==============");

			do
			{
				foreach (var result in results.data)
				{
					Console.WriteLine($"{result.id} {result.attributes.Name}");
					if (result.attributes.Name == find)
						id = result.id;
				}
				results = await pco.Services.GetNextRequest<ServicesResponse>(results.links);
			} while (results != null);
			Console.WriteLine();

			return id;
		}

		private static async Task<string> GetTeam(PlanningCenter pco, string withId, string find)
		{
			var results = await pco.Services.GetTeamsByService_typeId(withId);
			var id = string.Empty;

			Console.WriteLine($"Teams");
			Console.WriteLine("======");

			do
			{
				foreach (var result in results.data)
				{
					Console.WriteLine($"{result.id} {result.attributes.Name}");
					if (result.attributes.Name == find)
						id = result.id;
				}
				results = await pco.Services.GetNextRequest<TeamsResponse>(results.links);
			} while (results != null);
			Console.WriteLine();

			return id;
		}

		private static async Task<string> GetTeamPositions(PlanningCenter pco, string withId)
		{
			var results = await pco.Services.GetTeamPositionsByTeamID(withId);
			var id = results.data.id;

			Console.WriteLine($"TeamPositions");
			Console.WriteLine("==============");
			Console.WriteLine($"{results.data.id} {results.data.attributes.Name}");

			Console.WriteLine();

			return id;
		}

		private static async Task<string> GetPeople(PlanningCenter pco, string withId)
		{
			var results = await pco.Services.GetPeoplesByTeamID(withId);
			var id = string.Empty;

			Console.WriteLine($"People");
			Console.WriteLine("=======");

			do
			{
				foreach (var result in results.data)
				{
					Console.WriteLine($"{result.id} {result.attributes.Name}");
				}
				results = await pco.Services.GetNextRequest<PeoplesResponse>(results.links);
			} while (results != null);
			Console.WriteLine();

			return id;
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
	}
}