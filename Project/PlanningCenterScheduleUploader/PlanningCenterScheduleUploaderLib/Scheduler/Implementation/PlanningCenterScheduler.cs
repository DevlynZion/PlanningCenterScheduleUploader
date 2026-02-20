using ClosedXML.Excel;
using PlanningCenterAPI;
using PlanningCenterAPI.Respone.Constant;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;

namespace PlanningCenterScheduleUploaderLib.Scheduler.Implementation
{
	/// <summary>
	/// Submits the the scheduling data tp planning centure.
	/// </summary>
	public class PlanningCenterScheduler
	{
		private const string ServiceTypeConfigName = "Service Type";
		private const string TeamConfigName = "Team";
		private const string DataFormat = "d MMM yyyy";

		private ScheduleContext scheduleContext;

		public PlanningCenterScheduler(ScheduleContext scheduleContext)
		{
			this.scheduleContext = scheduleContext;
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
				foreach (var plan in scheduleContext.ScheduleDates)
				{
					var results = await pco.Services.GetPlanAssignments(scheduleContext.CachedManager.ServiceTypeId, scheduleContext.CachedManager.GetPlan(plan.Value), scheduleContext.CachedManager.TeamId);

					foreach (var result in results.data)
						await pco.Services.DeletePlanAssignments(scheduleContext.CachedManager.ServiceTypeId, scheduleContext.CachedManager.GetPlan(plan.Value), result.id);
				}
			}
		}

		public async Task SubmitScheduling()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				foreach (var assignment in scheduleContext.Assignments)
				{
					if (string.IsNullOrWhiteSpace(assignment.PersonName.Value))
						continue;

					var planId = scheduleContext.CachedManager.GetPlan(assignment.Date);
					await pco.Services.AddScheduleTeamMembers(scheduleContext.CachedManager.ServiceTypeId, planId, scheduleContext.CachedManager.TeamId, assignment.Role, scheduleContext.CachedManager.GetPerson(assignment.PersonName.Value));
				}
			}
		}

		private async Task CheckServiceType(PlanningCenter pco)
		{
			var serviceTypeId = await GetServiceTypeId(pco);

			if (string.IsNullOrEmpty(serviceTypeId))
			{
				var message = $"The {ServiceTypeConfigName} called {scheduleContext.Configs[ServiceTypeConfigName].Value} in the Config tab, does not exist on Planning Center";
				scheduleContext.Errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					CellCoordinate = scheduleContext.Configs[ServiceTypeConfigName],
					Message = message
				});
				throw new ArgumentException(message);
			}
		}

		private async Task CheckPlan(PlanningCenter pco)
		{
			var planIds = await GetPlans(pco, scheduleContext.CachedManager.ServiceTypeId);

			foreach (var plan in scheduleContext.ScheduleDates)
			{
				if(!planIds.ContainsKey(plan.Value))
				{
					var message = $"The Date {plan.Value.ToString(DataFormat)} in the Schedule tab, does not exist on Planning Center";
					scheduleContext.Errors.Add(new ScheduleErrors()
					{
						ErrorLevel = ErrorLevel.Warnning,
						CellCoordinate = plan,
						Message = message
					});
				}
			}
		}

		private async Task CheckTeam(PlanningCenter pco)
		{
			var teamId = await GetTeameId(pco, scheduleContext.CachedManager.ServiceTypeId);

			if(string.IsNullOrEmpty(teamId))
			{
				var message = $"The {TeamConfigName} called {scheduleContext.Configs[TeamConfigName].Value} in the Config tab, does not exist on Planning Center";
				scheduleContext.Errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					CellCoordinate = scheduleContext.Configs[TeamConfigName],
					Message = message
				});
				throw new ArgumentException(message);
			}
		}

		private async Task CheckRoles(PlanningCenter pco)
		{
			var roleIds = await GetRoles(pco, scheduleContext.CachedManager.ServiceTypeId, scheduleContext.CachedManager.TeamId);

			foreach (var role in scheduleContext.ScheduleRoles)
			{
				if(!roleIds.ContainsKey(role.Value.Value))
				{
					var message = $"The Role called {role.Value.Value} in the Schedule tab, does not exist on Planning Center";
					scheduleContext.Errors.Add(new ScheduleErrors()
					{
						ErrorLevel = ErrorLevel.Warnning,
						CellCoordinate = role.Value,
						Message = message
					});
				}
			}
		}

		private async Task CheckPeople(PlanningCenter pco)
		{
			var personIds = await GetPeopleInTeam(pco, scheduleContext.CachedManager.TeamId);

			foreach(var person in scheduleContext.Assignments.Select(a => a.PersonName.Value).Distinct())
			{
				if (string.IsNullOrWhiteSpace(person))
					continue;

				if (!personIds.ContainsKey(person))
				{
					foreach(var personCell in scheduleContext.Assignments.Where(a => a.PersonName.Value == person))
					{
						var message = $"The Person called {person} in the Schedule tab, does not exist on Planning Center";
						scheduleContext.Errors.Add(new ScheduleErrors()
						{
							ErrorLevel = ErrorLevel.Error,
							CellCoordinate = personCell.PersonName,
							Message = message
						});
					}
				}
			}
		}

		private async Task CheckPeopleInRoles(PlanningCenter pco)
		{
			// TODO: May or May not need will see later
		}

		private async Task CheckForPersonsBlockedOutDays(PlanningCenter pco)
		{
			var personIds = await GetPeopleInTeam(pco, scheduleContext.CachedManager.TeamId);

			foreach (var person in personIds)
			{
				var personBlockedOutDaysResults = await pco.Services.GetPersonsBlockoutDays(person.Value);
				// TODO: Redo Model; to allow to keep the IDs and Cell data so we can minimize query fetching.
				//       May also need other object to cache results from fetches.


			}
		}

		private async Task CheckIfPersonsAreNotAssignedOnOtherTeams(PlanningCenter pco)
		{

		}

		private async Task<string> GetServiceTypeId(PlanningCenter pco)
		{
			if(!string.IsNullOrWhiteSpace(scheduleContext.CachedManager.ServiceTypeId))
				return scheduleContext.CachedManager.ServiceTypeId;

			if (!scheduleContext.Configs.TryGetValue(ServiceTypeConfigName, out CellValue<string> configValue))
			{
				var message = $"Could not find the {ServiceTypeConfigName} config in the Config tab";
				scheduleContext.Errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					Message = message
				});
				throw new ArgumentException(message);
			}

			if(!configValue.HasValue)
			{
				var message = $"The {ServiceTypeConfigName} has not been set in the Config tab";
				scheduleContext.Errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					CellCoordinate = configValue,
					Message = message
				});
				throw new ArgumentException(message);
			}

			var serviceType = await pco.Services.GetServiceTypesByName(configValue.Value);

			if (!serviceType.data.Any())
				return string.Empty;

			scheduleContext.CachedManager.ServiceTypeId = serviceType.data.First().id;

			return scheduleContext.CachedManager.ServiceTypeId;
		}

		private async Task<string> GetTeameId(PlanningCenter pco, string serviceTypeId)
		{
			if(!string.IsNullOrEmpty(scheduleContext.CachedManager.TeamId))
				return scheduleContext.CachedManager.TeamId;

			if (!scheduleContext.Configs.TryGetValue(TeamConfigName, out CellValue<string> configValue))
			{
				var message = $"Could not find the {TeamConfigName} config in the Config tab";
				scheduleContext.Errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					Message = message
				});
				throw new ArgumentException(message);
			}

			if (!configValue.HasValue)
			{
				var message = $"The {TeamConfigName} has not been set in the Config tab";
				scheduleContext.Errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					Message = message
				});
				throw new ArgumentException(message);
			}

			var teamId = await pco.Services.GetTeamByName(serviceTypeId, configValue.Value);

			if (!teamId.data.Any())
				return string.Empty;

			scheduleContext.CachedManager.TeamId = teamId.data.First().id;

			return scheduleContext.CachedManager.TeamId;
		}

		private async Task<Dictionary<DateTime, string>> GetPlans(PlanningCenter pco, string serviceTypeId)
		{
			if (scheduleContext.CachedManager.AnyPlans())
				return scheduleContext.CachedManager.GetPlans();

			var results = await pco.Services.GetPlans(serviceTypeId);
			do
			{
				foreach (var result in results.data)
					scheduleContext.CachedManager.AddPlan(result.attributes.sort_date, result.id);

				results = await pco.Services.GetNextRequest<GetPlansResponse.Rootobject>(results.links);
			} while (results != null);

			return scheduleContext.CachedManager.GetPlans();
		}

		private async Task<Dictionary<string, string>> GetRoles(PlanningCenter pco, string serviceTypeId, string teamId)
		{
			if (scheduleContext.CachedManager.AnyRoles())
				return scheduleContext.CachedManager.GetRoles();

			var results = await pco.Services.GetTeamPositionsByTeamId(teamId);
				foreach (var result in results.included)
				{
					var teamPosition = await pco.Services.GetTeamPositionByServiceTypeIdTeamPositionsId(serviceTypeId, result.id);
					scheduleContext.CachedManager.AddRole(teamPosition.data.attributes.name, teamPosition.data.id);
				}
			
			return scheduleContext.CachedManager.GetRoles();
		}

		private async Task<Dictionary<string, string>> GetPeopleInTeam(PlanningCenter pco, string teamId)
		{
			if (scheduleContext.CachedManager.AnyPersons())
				return scheduleContext.CachedManager.GetPersons();

			var results = await pco.Services.GetPeoplesByTeamId(teamId);

			do
			{
				foreach (var result in results.data)
				{
					scheduleContext.CachedManager.AddPerson(result.attributes.full_name, result.id);
				}
				results = await pco.Services.GetNextRequest<GetPeoplesByTeamIdRespone.Rootobject>(results.links);
			} while (results != null);

			return scheduleContext.CachedManager.GetPersons();
		}
	}
}
