using PlanningCenterAPI;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;

namespace PlanningCenterScheduleUploaderLib.Validation.Implementation.PlanningCenterValidation
{
	public class RoleValidationStep : IPipelineStep<ScheduleContext>
	{
		public bool CanContine { get; }

		private PlanningCenter pco;

		public RoleValidationStep(PlanningCenter pco, bool canContine)
		{
			this.pco = pco;
			CanContine = canContine;
		}

		public async Task<ValidationResult> ProcessAsync(ScheduleContext input)
		{
			var result = new ValidationResult();

			var errors = await CheckRoles(pco, input);

			result.AddErrors(errors);

			return result;
		}

		private async Task<List<ScheduleErrors>> CheckRoles(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var errors = new List<ScheduleErrors>();
			var roleIds = await GetRoles(pco, scheduleContext);
            var rolesToRemove = new Dictionary<string, CellValue<string>>();

            foreach (var role in scheduleContext.ScheduleRoles)
			{
				if (!roleIds.ContainsKey(role.Value.Value))
				{
					var message = $"The Role called {role.Value.Value} in the Schedule tab, does not exist on Planning Center";
					errors.Add(new ScheduleErrors()
					{
						ErrorLevel = ErrorLevel.Warnning,
						CellCoordinate = role.Value,
						Message = message
					});

                    rolesToRemove.Add(role.Key, role.Value);
                }
			}

			RemoveMissingRoles(scheduleContext, rolesToRemove);

            return errors;
		}

		private async Task<Dictionary<string, string>> GetRoles(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var results = await pco.Services.GetTeamPositionsByTeamId(scheduleContext.CachedManager.TeamId);
			foreach (var result in results.included)
			{
				var teamPosition = await pco.Services.GetTeamPositionByServiceTypeIdTeamPositionsId(scheduleContext.CachedManager.ServiceTypeId, result.id);
				scheduleContext.CachedManager.AddRole(teamPosition.data.attributes.name, teamPosition.data.id);
			}

			return scheduleContext.CachedManager.GetRoles();
		}

        private void RemoveMissingRoles(ScheduleContext scheduleContext, Dictionary<string, CellValue<string>> rolesToRemove)
        {
            foreach (var role in rolesToRemove)
            {
                var removeRoles = scheduleContext.Assignments.Where(a => a.Role == role.Value.Value);

                while (removeRoles.Any())
                    scheduleContext.Assignments.Remove(removeRoles.First());

                scheduleContext.ScheduleRoles.Remove(role.Key);
            }
        }
    }
}
