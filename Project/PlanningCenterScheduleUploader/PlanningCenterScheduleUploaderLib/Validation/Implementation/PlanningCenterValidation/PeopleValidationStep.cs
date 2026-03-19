using PlanningCenterAPI;
using PlanningCenterAPI.Respone.Constant;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;

namespace PlanningCenterScheduleUploaderLib.Validation.Implementation.PlanningCenterValidation
{
	public class PeopleValidationStep : IPipelineStep<ScheduleContext>
	{
		public bool CanContine { get; }

		private PlanningCenter pco;

		public PeopleValidationStep(PlanningCenter pco, bool canContine)
		{
			this.pco = pco;
			CanContine = canContine;
		}

		public async Task<ValidationResult> ProcessAsync(ScheduleContext input)
		{
			var result = new ValidationResult();

			var errors = await CheckPeople(pco, input);

			result.AddErrors(errors);

			return result;
		}

		private async Task<List<ScheduleErrors>> CheckPeople(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var errors = new List<ScheduleErrors>();

			var personIds = await GetPeopleInTeam(pco, scheduleContext);

			foreach (var person in scheduleContext.Assignments.Select(a => a.PersonName.Value).Distinct())
			{
				if (string.IsNullOrWhiteSpace(person))
					continue;

				if (!personIds.ContainsKey(person))
				{
					foreach (var personCell in scheduleContext.Assignments.Where(a => a.PersonName.Value == person))
					{
						var message = $"The Person called {person} in the Schedule tab, does not exist on Planning Center";
						errors.Add(new ScheduleErrors()
						{
							ErrorLevel = ErrorLevel.Error,
							CellCoordinate = personCell.PersonName,
							Message = message
						});
					}
				}
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
