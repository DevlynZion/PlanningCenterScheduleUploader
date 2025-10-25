using PlanningCenterAPI;
using PlanningCenterAPI.Core.Interface;
using PlanningCenterAPI.Helper;
using PlanningCenterAPI.Type;
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

		static Dictionary<string, ServiceAttribute> ServiceTypes = new Dictionary<string, ServiceAttribute>();
		static Dictionary<string, ServiceAttribute> PlanTemplates = new Dictionary<string, ServiceAttribute>();
		static Dictionary<string, TeamAttibute> Team = new Dictionary<string, TeamAttibute>();
		static Dictionary<string, TeamPositionAttribute> TeamPositions = new Dictionary<string, TeamPositionAttribute>();
		static Dictionary<string, PeopleAttribute> PeopleOnTeam = new Dictionary<string, PeopleAttribute>();
		static Dictionary<string, TeamPositionAttribute> TeamPositionsOnTeam = new Dictionary<string, TeamPositionAttribute>();

		static void Main(string[] args)
		{
			TestQueryNeededPosition().Wait();
			//StressTest1().Wait();
			//TestPeople();


			Console.WriteLine($"Program Done");
			Console.ReadKey();
		}

		static async Task TestQueryNeededPosition()
		{
			using (PlanningCenter pco = new PlanningCenter(true, true, true))
			{
				var serviceTypes = await pco.Services.GetService_types();
				var serviceTypeID = await GetEntity(pco, serviceTypes, "ServiceTypes", ServiceTypes, "1235720");
				if (string.IsNullOrEmpty(serviceTypeID))
				{
					return;
				}

				//var planTemplates = await pco.Services.GetPlan_templatesByService_typeId(serviceTypeID);
				//var planTemplateID = await GetEntity(pco, planTemplates, "PlanTemplates", PlanTemplates);
				//if (string.IsNullOrEmpty(planTemplateID))
				//{
				//	return;
				//}

				var teams = await pco.Services.GetTeamsByService_typeId(serviceTypeID);
				var teamID = await GetEntity(pco, teams, "Team", Team, "5948513");
				if (string.IsNullOrEmpty(teamID))
				{
					return;
				}


				//var teamPositions = await pco.Services.GetTeamPositionsByService_typeId(serviceTypeID);
				//var teamPositionsIDs = await GetEntities(pco, teamPositions, "TeamPositions", TeamPositions, "31782683,31782685,31782687,31782881");
				//if (teamPositionsIDs.Count == 0)
				//{
				//	return;
				//}

				var peopleOnTeam = await pco.Services.GetPeoplesByTeamID(teamID);
				var peopleOnTeamIds = await GetEntities(pco, peopleOnTeam, "People", PeopleOnTeam);
				//if (peopleOnTeamIds.Count == 0)
				//{
				//	return;
				//}

				var teamPositionsOnTeam = await pco.Services.GetTeamPositionsByTeamID(teamID);
				var teamPositionsOnTeamIds = await GetEntities(pco, teamPositionsOnTeam, "TeamPositions", TeamPositionsOnTeam);
				//if (peopleOnTeamIds.Count == 0)
				//{
				//	return;
				//}

				//https://api.planningcenteronline.com/services/v2/teams?include=person_team_position_assignments&where[name]=Live Stream
				//https://api.planningcenteronline.com/services/v2/service_types/1235720/plans?where[id]=77741849

			}
		}

		private static async Task<string> GetEntity<R, D>(PlanningCenter pco, R results, string entityName, Dictionary<string, D> collection, string defaultChoice = null) where R : RootBase<D> where D : IAttribute
		{
			string id;
			collection = await GetResults<R, D>(pco, results, entityName);

			if (collection.Count > 1)
			{
				id = GetUserSelection(entityName, defaultChoice);
				if (id == null)
				{
					return string.Empty;
				}
			}
			else
			{
				id = GetUserSelection(entityName, collection.First().Key);
				if (id == null)
				{
					return string.Empty;
				}
			}

			return id;
		}

		private static async Task<List<string>> GetEntities<R, D>(PlanningCenter pco, R results, string entityName, Dictionary<string, D> collection, string defaultChoice = null) where R : RootBase<D> where D : IAttribute
		{
			collection = await GetResults<R, D>(pco, results, entityName);

			if (collection.Count > 1)
			{
				return GetUserMutliSelection(entityName, defaultChoice);
			}
			else
			{
				return GetUserMutliSelection(entityName, collection.First().Key);
			}
		}

		static async Task<Dictionary<string, D>> GetResults<R, D>(PlanningCenter pco, R results, string enityName) where R : RootBase<D> where D : IAttribute
		{
			Dictionary<string, D> collection = new Dictionary<string, D>();

			Console.WriteLine(enityName);
			Console.WriteLine("=".PadRight(enityName.Length, '='));

			do
			{
				foreach (var result in results.data)
				{
					collection.Add(result.id, result.attributes);
					Console.WriteLine($"{result.id} {result.attributes.Name}");
				}
				results = await pco.Services.GetNextRequest<R>(results.links);
			} while (results != null);

			Console.WriteLine();

			return collection;
		}

		static string GetUserSelection(string entityName, string defaultChoice = null)
		{
			if (string.IsNullOrEmpty(defaultChoice))
			{
				Console.WriteLine("Enter ID:");
			}
			else
			{
				Console.WriteLine($"Enter ID or default will be [{defaultChoice}]:");
			}

			var idString = Console.ReadLine();

			if (string.IsNullOrEmpty(idString) && string.IsNullOrEmpty(defaultChoice))
			{
				Console.WriteLine($"Could not find {entityName}");
				return string.Empty;
			}
			else if(string.IsNullOrEmpty(idString) && !string.IsNullOrEmpty(defaultChoice))
			{
				return defaultChoice;
			}
			else
			{
				Console.WriteLine();
				return idString;
			}
		}

		static List<string> GetUserMutliSelection(string entityName, string defaultChoice = null)
		{
			if (string.IsNullOrEmpty(defaultChoice))
			{
				Console.WriteLine("Enter IDs:");
			}
			else
			{
				Console.WriteLine($"Enter IDs or default will be [{defaultChoice}]:");
			}

			var idsString = Console.ReadLine();

			if (string.IsNullOrEmpty(idsString) && string.IsNullOrEmpty(defaultChoice))
			{
				Console.WriteLine($"Could not find {entityName}");
				return new List<string>();
			}
			else if (string.IsNullOrEmpty(idsString) && !string.IsNullOrEmpty(defaultChoice))
			{
				return defaultChoice.Split(',').ToList();
			}
			else
			{
				Console.WriteLine();
				return idsString.Split(',').ToList();
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