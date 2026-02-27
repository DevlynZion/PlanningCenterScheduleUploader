using PlanningCenterAPI;
using PlanningCenterAPI.Respone.Constant;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;

namespace PlanningCenterScheduleUploaderLib.Validation.Implementation.PlanningCenterValidation
{
	public class PersonsBlockedOutDaysValidationStep : IPipelineStep<ScheduleContext>
	{
		public bool CanContine { get; }

		private PlanningCenter pco;

		public PersonsBlockedOutDaysValidationStep(PlanningCenter pco, bool canContine)
		{
			this.pco = pco;
			CanContine = canContine;
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

			var personIds = scheduleContext.CachedManager.GetPersons();

			foreach (var person in personIds)
			{
				var personBlockedOutDaysResults = await pco.Services.GetPersonsBlockoutDays(person.Value);
				do
				{
					foreach (var personBlockedOutDaysResult in personBlockedOutDaysResults.data)
					{
						var startDate = personBlockedOutDaysResult.attributes.starts_at;
						var endDate = personBlockedOutDaysResult.attributes.ends_at;

						if (!scheduleContext.PersonsBlockedDays.ContainsKey(person.Value))
							scheduleContext.PersonsBlockedDays.Add(person.Value, new List<PersonBlockDays>());

						var blockDay = new PersonBlockDays()
						{
							PersonID = person.Value,
							StartDate = startDate,
							EndDate = endDate
						};
						scheduleContext.PersonsBlockedDays[person.Value].Add(blockDay);
					}

					personBlockedOutDaysResults = await pco.Services.GetNextRequest<GetPersonsBlockoutDaysRespone.Rootobject>(personBlockedOutDaysResults.links);
				} while (personBlockedOutDaysResults != null);

			}

			// TODO: mark persons that are not allowed to be assigned

			return errors;
		}
	}
}
