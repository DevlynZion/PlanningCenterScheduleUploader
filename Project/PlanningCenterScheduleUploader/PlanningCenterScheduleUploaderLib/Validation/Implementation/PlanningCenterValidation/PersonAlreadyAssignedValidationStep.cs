using PlanningCenterAPI;
using PlanningCenterAPI.Respone.Constant;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using PlanningCenterScheduleUploaderLib.Scheduler.Core.Constant;

namespace PlanningCenterScheduleUploaderLib.Validation.Implementation.PlanningCenterValidation
{
	public class PersonAlreadyAssignedValidationStep : IPipelineStep<ScheduleContext>
	{
		public bool CanContine { get; }
		private PlanningCenter pco;

		public PersonAlreadyAssignedValidationStep(PlanningCenter pco, bool canContine)
		{
			this.pco = pco;
			CanContine = canContine;
		}
		public async Task<ValidationResult> ProcessAsync(ScheduleContext input)
		{
			var result = new ValidationResult();

			var errors = await CheckPersonsForOtherAssignements(pco, input);

			result.AddErrors(errors);

			return result;
		}

		private async Task<List<ScheduleErrors>> CheckPersonsForOtherAssignements(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var errors = new List<ScheduleErrors>();

			await GetPersonAssingments(pco, scheduleContext);

			foreach(var assignment in scheduleContext.Assignments)
			{
				var personName = assignment.PersonName.Value;
				var personId = scheduleContext.CachedManager.GetPerson(personName);
				var planId = scheduleContext.CachedManager.GetPlan(assignment.Date);

				if (!scheduleContext.PersonAssignments.ContainsKey(personId))
					continue;
 
				if(scheduleContext.PersonAssignments[personId].Any(pa => pa.PlanId == planId))
				{
					var message = $"Person {personName} in the Schedule tab, is already assigned to other team on Date {assignment.Date.ToString(PlanningCenterConstants.DataFormat)} on Planning Center.";
					errors.Add(new ScheduleErrors()
					{
						ErrorLevel = ErrorLevel.Warnning,
						CellCoordinate = assignment.PersonName,
						Message = message
					});
				}
			}

			return errors;
		}

		private static async Task GetPersonAssingments(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var personIds = scheduleContext.CachedManager.GetPersons();

			foreach (var person in personIds)
			{
				var personOtherAssignmentsResults = await pco.Services.GetPersonsAssingments(person.Value);
				do
				{
					foreach (var personOtherAssignmentsResult in personOtherAssignmentsResults.data)
					{
						if (personOtherAssignmentsResult.relationships.team.data.id == scheduleContext.CachedManager.TeamId)
							continue;

						if (!scheduleContext.PersonAssignments.ContainsKey(person.Value))
							scheduleContext.PersonAssignments.Add(person.Value, new List<PersonAssignment>());

						scheduleContext.PersonAssignments[person.Value].Add(new PersonAssignment()
						{
							PersonName = person.Key,
							Role = personOtherAssignmentsResult.attributes.team_position_name,
							PlanId = personOtherAssignmentsResult.relationships.plan.data.id,
							TeamId = personOtherAssignmentsResult.relationships.team.data.id,
						});
					}
					personOtherAssignmentsResults = await pco.Services.GetNextRequest<GetPersonsAssingmentsResponse.Rootobject>(personOtherAssignmentsResults.links);
				} while (personOtherAssignmentsResults != null);
			}
		}
	}
}
