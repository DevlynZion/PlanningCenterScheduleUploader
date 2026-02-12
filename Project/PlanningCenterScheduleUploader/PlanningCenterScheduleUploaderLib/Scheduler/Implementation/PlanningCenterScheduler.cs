using ClosedXML.Excel;
using PlanningCenterAPI;
using PlanningCenterAPI.Respone.Constant;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Scheduler.Implementation
{
	/// <summary>
	/// Submits the the scheduling data tp planning centure.
	/// </summary>
	public class PlanningCenterScheduler
	{
		private const string ServiceTypeConfigName = "Service Type";
		private const string TeamConfigName = "Team";

		private IScheduleModel scheduleModel;
		private Dictionary<string, string> personIds;
		private Dictionary<string, string> planIds;
		private Dictionary<string, string> roleIds;

		private string serviceTypeId;
		private string teamId;

		public PlanningCenterScheduler(IScheduleModel scheduleModel)
		{
			this.scheduleModel = scheduleModel;
			personIds = new Dictionary<string, string>();
			planIds = new Dictionary<string, string>();
			roleIds = new Dictionary<string, string>();
		}

		public async Task DoChecks()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				// 2.1 Does Service Type exist on Planning Centre.
				await CheckServiceType(pco);
				// 2.2 Does Plans exist on Planning Centre.
				await CheckPlan(pco);
				// 2.3 Does Team exist on Planning Centre.
				await CheckTeam(pco);
				// 2.4 Does Roles exist on Planning Centre.
				await CheckRoles(pco);
				// 2.5 Does the people exist on Planning Centre.
				await CheckPeople(pco);
				// 2.6 Does the people exist in their assign roles on Planning Centre(Not sure if needed).
				await CheckPeopleInRoles(pco);
				// 2.7 Check for person blockouts days.
				await CheckForPersonsBlockedOutDays(pco);
				// 2.8 Check if person is assigned elsewhere.
				await CheckIfPersonsAreNotAssignedOnOtherTeams(pco);
			}
		}

		public async Task ClearPlans()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				foreach (var plan in scheduleModel.ScheduleAssignmentsModel.ScheduleAssignmentModel.Select(a => a.Date))
				{
					var results = await pco.Services.GetPlanAssignments(serviceTypeId, planIds[plan], teamId);

					foreach (var result in results.data)
						await pco.Services.DeletePlanAssignments(serviceTypeId, planIds[plan], result.id);
				}
			}
		}

		public async Task SubmitScheduling()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				foreach (var scheduleAssignment in scheduleModel.ScheduleAssignmentsModel.ScheduleAssignmentModel)
				{
					var planId = planIds[scheduleAssignment.Date.Value];
					foreach (var assignment in scheduleAssignment.RolePersons)
						await pco.Services.AddScheduleTeamMembers(serviceTypeId, planId, teamId, assignment.Key, await GetPersonID(pco, assignment.Value));
				}
			}
		}

		private async Task CheckServiceType(PlanningCenter pco)
		{
			serviceTypeId = await GetServiceTypeId(pco);

			if (string.IsNullOrEmpty(serviceTypeId))
			{
				var message = $"The {ServiceTypeConfigName} called {scheduleModel.ScheduleConfigModel.Config[ServiceTypeConfigName].Value} in the Config tab, does not exist on Planning Center";
				scheduleModel.Errors.Add(message);
				var serviceTypeConfigCell = scheduleModel.ScheduleConfigModel.Config[ServiceTypeConfigName];
				serviceTypeConfigCell.ChangeColourTo = XLColor.Red;
				scheduleModel.CellsToChange.Add(serviceTypeConfigCell);
				throw new ArgumentException(message);
			}
		}

		private async Task CheckPlan(PlanningCenter pco)
		{
			planIds = await GetPlans(pco, serviceTypeId);

			foreach (var plan in scheduleModel.ScheduleAssignmentsModel.ScheduleAssignmentModel.Select(a => a.Date).Distinct())
			{
				if(!planIds.ContainsKey(plan))
				{
					var message = $"The Date {plan} in the Schedule tab, does not exist on Planning Center";
					scheduleModel.Errors.Add(message);
					plan.ChangeColourTo = XLColor.Red;
					scheduleModel.CellsToChange.Add(plan);
				}
			}
		}

		private async Task CheckTeam(PlanningCenter pco)
		{
			teamId = await GetTeameId(pco, serviceTypeId);

			if(string.IsNullOrEmpty(teamId))
			{
				var message = $"The {TeamConfigName} called {scheduleModel.ScheduleConfigModel.Config[TeamConfigName].Value} in the Config tab, does not exist on Planning Center";
				scheduleModel.Errors.Add(message);
				var teamConfigCell = scheduleModel.ScheduleConfigModel.Config[TeamConfigName];
				teamConfigCell.ChangeColourTo = XLColor.Red;
				scheduleModel.CellsToChange.Add(teamConfigCell);
				throw new ArgumentException(message);
			}
		}

		private async Task CheckRoles(PlanningCenter pco)
		{
			roleIds = await GetRoles(pco, teamId);

			foreach (var role in scheduleModel.ScheduleAssignmentsModel.ScheduleAssignmentModel.SelectMany(a => a.RolePersons.Keys).Distinct())
			{
				if(!roleIds.ContainsKey(role))
				{
					var message = $"The Role called {role} in the Schedule tab, does not exist on Planning Center";
					scheduleModel.Errors.Add(message);
					role.ChangeColourTo = XLColor.Red;
					scheduleModel.CellsToChange.Add(role);
				}
			}
		}

		private async Task CheckPeople(PlanningCenter pco)
		{
			personIds = await GetPeopleInTeam(pco, teamId);

			foreach(var person in scheduleModel.ScheduleAssignmentsModel.ScheduleAssignmentModel.SelectMany(a => a.RolePersons.Values))
			{
				if (!personIds.ContainsKey(person))
				{
					var message = $"The Person called {person} in the Schedule tab, does not exist on Planning Center";
					scheduleModel.Errors.Add(message);
					person.ChangeColourTo = XLColor.Red;
					scheduleModel.CellsToChange.Add(person);
				}
			}
		}

		private async Task CheckPeopleInRoles(PlanningCenter pco)
		{
			// TODO: May or May not need will see later
		}

		private async Task CheckForPersonsBlockedOutDays(PlanningCenter pco)
		{
			foreach (var person in scheduleModel.ScheduleAssignmentsModel.ScheduleAssignmentModel.SelectMany(a => a.RolePersons.Values.Select(p => p.Value)).Distinct())
			{
				var personBlockedOutDaysResults = await pco.Services.GetPersonsBlockoutDays(personIds[person]);
				// TODO: Redo Model; to allow to keep the IDs and Cell data so we can minimize query fetching.
				//       May also need other object to cache results from fetches.
			}
		}

		private async Task CheckIfPersonsAreNotAssignedOnOtherTeams(PlanningCenter pco)
		{

		}

		private async Task<string> GetServiceTypeId(PlanningCenter pco)
		{
			var serviceType = await pco.Services.GetServiceTypesByName(scheduleModel.ScheduleConfigModel.Config[ServiceTypeConfigName].Value);

			if (!serviceType.data.Any())
				return string.Empty;

			return serviceType.data.FirstOrDefault().id;
		}

		private async Task<string> GetTeameId(PlanningCenter pco, string serviceTypeId)
		{
			var teamId = await pco.Services.GetTeamByName(serviceTypeId, scheduleModel.ScheduleConfigModel.Config[TeamConfigName].Value);

			if (!teamId.data.Any())
				return string.Empty;

			return teamId.data.FirstOrDefault().id;
		}

		private async Task<Dictionary<string, string>> GetPlans(PlanningCenter pco, string serviceTypeId)
		{
			if (planIds.Count == 0)
			{
				var results = await pco.Services.GetPlans(serviceTypeId);
				do
				{
					foreach (var result in results.data)
						if (!planIds.ContainsKey(result.attributes.short_dates))
							planIds.Add(result.attributes.short_dates, result.id);

					results = await pco.Services.GetNextRequest<GetPlansResponse.Rootobject>(results.links);
				} while (results != null);
			}

			return planIds;
		}

		private async Task<Dictionary<string, string>> GetRoles(PlanningCenter pco, string teamId)
		{
			if (roleIds.Count == 0)
			{
				var results = await pco.Services.GetTeamPositionsByTeamId(teamId);
				foreach (var result in results.included)
				{
					var teamPosition = await pco.Services.GetTeamPositionByServiceTypeIdTeamPositionsId(serviceTypeId, result.id);
					if (!roleIds.ContainsKey(teamPosition.data.attributes.name))
						roleIds.Add(teamPosition.data.attributes.name, teamPosition.data.id);
				}
			}
			return roleIds;
		}

		private async Task<Dictionary<string, string>> GetPeopleInTeam(PlanningCenter pco, string teamId)
		{
			var results = await pco.Services.GetPeoplesByTeamId(teamId);

			do
			{
				foreach (var result in results.data)
				{
					if (!personIds.ContainsKey(result.attributes.full_name))
						personIds.Add(result.attributes.full_name, result.id);
				}
				results = await pco.Services.GetNextRequest<GetPeoplesByTeamIdRespone.Rootobject>(results.links);
			} while (results != null);

			return personIds;
		}

		private async Task<string> GetPersonID(PlanningCenter pco, string fullName)
		{
			if(personIds.ContainsKey(fullName))
			{
				return personIds[fullName];
			}
			else
			{
				var results = await pco.Services.GetPersonByName(fullName);
				var person = results.data.First();
				personIds.Add(fullName, person.id);
				return person.id;
			}
		}
	}
}
