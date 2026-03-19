using PlanningCenterAPI;
using PlanningCenterAPI.Respone.Constant;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using PlanningCenterScheduleUploaderLib.Scheduler.Core.Constant;

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

			await GetBlockedDays(pco, scheduleContext);

			var removeAssignments = new List<ScheduleAssignment>();

			foreach(var assignment in scheduleContext.Assignments)
			{
				var person = assignment.PersonName.Value;
				var personId = scheduleContext.CachedManager.GetPerson(person);

				if (!scheduleContext.PersonsBlockedDays.ContainsKey(personId))
					continue;

				var blockDays = scheduleContext.PersonsBlockedDays[personId];
				if (!blockDays.Any())
					continue;

				foreach (var block in blockDays)
				{
					if(assignment.Date < block.StartDate || assignment.Date > block.EndDate)
						continue;

					var message = $"Person {person} in the Schedule tab, has blocked out the from {block.StartDate.ToString(PlanningCenterConstants.DataFormat)} to {block.EndDate.ToString(PlanningCenterConstants.DataFormat)} on Planning Center. Therefore cannot serve on {assignment.Date.ToString(PlanningCenterConstants.DataFormat)}.";
					errors.Add(new ScheduleErrors()
					{
						ErrorLevel = ErrorLevel.Warnning,
						CellCoordinate = assignment.PersonName,
						Message = message
					});

					removeAssignments.Add(assignment);
				}
			}

			foreach (var assignment in removeAssignments)
				scheduleContext.Assignments.Remove(assignment);

			return errors;
		}

		private static async Task GetBlockedDays(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var personIds = scheduleContext.CachedManager.GetPersons();

			foreach (var person in personIds)
			{
				var personBlockedOutDaysResults = await pco.Services.GetPersonsBlockoutDays(person.Value);
				do
				{
					foreach (var personBlockedOutDaysResult in personBlockedOutDaysResults.data)
					{
						var startDate = DateOnly.FromDateTime(personBlockedOutDaysResult.attributes.starts_at);
						var endDate = DateOnly.FromDateTime(personBlockedOutDaysResult.attributes.ends_at);

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
		}
	}
}
