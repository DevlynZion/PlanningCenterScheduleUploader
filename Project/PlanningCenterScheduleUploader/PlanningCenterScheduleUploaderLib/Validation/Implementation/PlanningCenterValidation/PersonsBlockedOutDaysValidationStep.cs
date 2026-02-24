using PlanningCenterAPI;
using PlanningCenterAPI.Respone.Constant;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;

namespace PlanningCenterScheduleUploaderLib.Validation.Implementation.PlanningCenterValidation
{
	public class PersonsBlockedOutDaysValidationStep : IPipelineStep<ScheduleContext>
	{
		private PlanningCenter pco;

		public PersonsBlockedOutDaysValidationStep(PlanningCenter pco)
		{
			this.pco = pco;
		}

		public async Task<ValidationResult> ProcessAsync(ScheduleContext input)
		{
			var result = new ValidationResult();

			var errors = await CheckForPersonsBlockedOutDays(pco, input);

			result.AddErrors(errors);

			return result;
		}

		private async Task<List<ScheduleErrors>> CheckForPersonsBlockedOutDays(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var errors = new List<ScheduleErrors>();

			var personIds = await GetPeopleInTeam(pco, scheduleContext);

			foreach (var person in personIds)
			{
				var personBlockedOutDaysResults = await pco.Services.GetPersonsBlockoutDays(person.Value);
				// TODO: Redo Model; to allow to keep the IDs and Cell data so we can minimize query fetching.
				//       May also need other object to cache results from fetches.


			}

			return errors;
		}

		private async Task<Dictionary<string, string>> GetPeopleInTeam(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var results = await pco.Services.GetPeoplesByTeamId(scheduleContext.CachedManager.TeamId);

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
