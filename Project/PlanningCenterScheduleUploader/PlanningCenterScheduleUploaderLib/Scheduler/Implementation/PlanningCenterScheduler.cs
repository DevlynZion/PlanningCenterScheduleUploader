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

		public PlanningCenterScheduler(IScheduleModel scheduleModel)
		{
			this.scheduleModel = scheduleModel;
			personIds = new Dictionary<string, string>();
		}

		public async Task SubmitScheduling()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				var serviceTypeId = await GetServiceTypeId(pco);
				var teamId = await GetTeameId(pco, serviceTypeId);
				var planIds = await GetPlans(pco, serviceTypeId);

				foreach (var scheduleAssignment in scheduleModel.ScheduleAssignmentsModel.ScheduleAssignmentModel)
				{
					var planId = planIds[scheduleAssignment.Date];
					foreach (var assignment in scheduleAssignment.RolePersons)
						await pco.Services.AddScheduleTeamMembers(serviceTypeId, planId, teamId, assignment.Key, await GetPersonID(pco, assignment.Value));

				}
			}
		}

		private async Task<string> GetServiceTypeId(PlanningCenter pco)
		{
			var serviceType = await pco.Services.GetServiceTypesByName(scheduleModel.ScheduleConfigModel.Config[ServiceTypeConfigName]);
			return serviceType.data.First().id;
		}

		private async Task<string> GetTeameId(PlanningCenter pco, string serviceTypeId)
		{
			var teamId = await pco.Services.GetTeamByName(serviceTypeId, scheduleModel.ScheduleConfigModel.Config[TeamConfigName]);
			return teamId.data.First().id;
		}

		private async Task<Dictionary<string, string>> GetPlans(PlanningCenter pco, string serviceTypeId)
		{
			var planIds = new Dictionary<string, string>();
			var results = await pco.Services.GetPlans(serviceTypeId);
			do
			{
				foreach (var result in results.data)
					if (!planIds.ContainsKey(result.attributes.short_dates))
						planIds.Add(result.attributes.short_dates, result.id);
				
				results = await pco.Services.GetNextRequest<GetPlansResponse.Rootobject>(results.links);
			} while (results != null);

			return planIds;
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
