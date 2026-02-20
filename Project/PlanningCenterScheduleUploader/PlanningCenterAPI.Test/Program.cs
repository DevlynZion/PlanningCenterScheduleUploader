using PlanningCenterAPI;
using PlanningCenterAPI.Respone.Constant;
using PlanningCenterScheduleUploaderLib;
using PlanningCenterScheduleUploaderLib.Process.Implementation;

namespace PlanningCenterScheduleUploader
{
	internal class Program
	{
		const int StressIterations = 150;
		const int ProgramWait = 100000;


		static void Main(string[] args)
		{
			TestExcelProcessor().Wait();

			//TestQueryNeededPosition().Wait();
			//StressTest1().Wait();

			Console.WriteLine($"Program Done");
			Console.ReadKey();
		}

		static async Task TestExcelProcessor()
		{
			var excelProcessor = new ExcelProcessor(@"./Schedule.xlsx");
			PlanningCenterManager planningCenterManager = new PlanningCenterManager(excelProcessor);
			try
			{
				await planningCenterManager.Start();
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			finally
			{
				if (planningCenterManager.AnyErrors)
				{
					Console.WriteLine("Error");
					Console.WriteLine("=====");
					foreach (var error in planningCenterManager.Errors)
						Console.WriteLine($"{error.ErrorLevel.ToString()} - [{error.CellCoordinate.TabName}, {error.CellCoordinate.RowNumber}, {error.CellCoordinate.RowNumber}] {error.Message}");
				}
			}
		}

		static async Task TestQueryNeededPosition()
		{
			using (PlanningCenter pco = new PlanningCenter(true, true, true))
			{
				var serviceTypeId = await GetServiceType(pco, "Sunday and Other Services");
				var planTemplateID = await GetPlanTemplate(pco, serviceTypeId, "Sunday Service");
				await GetPlans(pco, serviceTypeId);
				var teamId = await GetTeam(pco, serviceTypeId, "Live Stream");
				var teamPositionIds = await GetTeamPositions(pco, teamId, serviceTypeId);
				var peopleIds = await GetPeople(pco, teamId);

				await AddAssignment(pco, serviceTypeId, "84546567", teamId, "Editor", peopleIds);
			}
		}

		private static async Task<string> GetServiceType(PlanningCenter pco, string find)
		{
			var results = await pco.Services.GetServiceTypes();
			var id = string.Empty;

			Console.WriteLine($"ServiceTypes");
			Console.WriteLine("=============");

			do
			{
				foreach (var result in results.data)
				{
					Console.WriteLine($"{result.id} {result.attributes.name}");
					if (result.attributes.name == find)
						id = result.id;
				}
				results = await pco.Services.GetNextRequest<GetServiceTypesResponse.Rootobject>(results.links);
			} while (results != null);
			Console.WriteLine();

			return id;
		}

		private static async Task<string> GetPlanTemplate(PlanningCenter pco, string withId, string find)
		{
			var results = await pco.Services.GetPlanTemplates(withId);
			var id = string.Empty;

			Console.WriteLine($"PlanTemplates");
			Console.WriteLine("==============");

			do
			{
				foreach (var result in results.data)
				{
					Console.WriteLine($"{result.id} {result.attributes.name}");
					if (result.attributes.name == find)
						id = result.id;
				}
				results = await pco.Services.GetNextRequest<GetPlanTemplatesRespone.Rootobject>(results.links);
			} while (results != null);
			Console.WriteLine();

			return id;
		}

		private static async Task GetPlans(PlanningCenter pco, string withId)
		{
			var results = await pco.Services.GetPlans(withId);

			Console.WriteLine($"Plans");
			Console.WriteLine("======");

			do
			{
				foreach (var result in results.data)
				{
					Console.WriteLine($"{result.id} {result.attributes.dates}");
				}
				results = await pco.Services.GetNextRequest<GetPlansResponse.Rootobject>(results.links);
			} while (results != null);
			Console.WriteLine();
		}

		private static async Task<string> GetTeam(PlanningCenter pco, string withId, string find)
		{
			var results = await pco.Services.GetTeams(withId);
			var id = string.Empty;

			Console.WriteLine($"Teams");
			Console.WriteLine("======");

			do
			{
				foreach (var result in results.data)
				{
					Console.WriteLine($"{result.id} {result.attributes.name}");
					if (result.attributes.name == find)
						id = result.id;
				}
				results = await pco.Services.GetNextRequest<GetTeamsResponse.Rootobject>(results.links);
			} while (results != null);
			Console.WriteLine();

			return id;
		}

		private static async Task<string> GetTeamPositions(PlanningCenter pco, string withId, string serviceTypeId)
		{
			var results = await pco.Services.GetTeamPositionsByTeamId(withId);
			var id = results.data.id;

			Console.WriteLine($"TeamPositions");
			Console.WriteLine("==============");
			foreach (var result in results.included)
			{
				var teamPosition = await pco.Services.GetTeamPositionByServiceTypeIdTeamPositionsId(serviceTypeId, result.id);

				Console.WriteLine($"{teamPosition.data.id} {teamPosition.data.attributes.name}");
			}

			Console.WriteLine();

			return id;
		}

		private static async Task<string> GetPeople(PlanningCenter pco, string withId)
		{
			var results = await pco.Services.GetPeoplesByTeamId(withId);
			var id = results.data.Where(p => p.attributes.full_name == "Devlyn van der Walt").First().id;

			Console.WriteLine($"People");
			Console.WriteLine("=======");

			do
			{
				foreach (var result in results.data)
				{
					Console.WriteLine($"{result.id} {result.attributes.full_name}");
				}
				results = await pco.Services.GetNextRequest<GetPeoplesByTeamIdRespone.Rootobject>(results.links);
			} while (results != null);
			Console.WriteLine();

			return id;
		}

		private static async Task AddAssignment(PlanningCenter pco, string serivesTypeId, string planId, string teamId, string teamPositionName, string peopleId)
		{
			var results = await pco.Services.AddScheduleTeamMembers(serivesTypeId, planId, teamId, teamPositionName, peopleId);

			Console.WriteLine($"AddAssignment!");
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
				Console.WriteLine($"{k} Data returned = {data.data.Count()}");
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