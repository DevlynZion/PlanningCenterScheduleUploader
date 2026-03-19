using PlanningCenterAPI;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using PlanningCenterScheduleUploaderLib.Scheduler.Core.Constant;

namespace PlanningCenterScheduleUploaderLib.Validation.Implementation.PlanningCenterValidation
{
	public class TeamValidationStep : IPipelineStep<ScheduleContext>
	{
		public bool CanContine { get; }

		private PlanningCenter pco;

		public TeamValidationStep(PlanningCenter pco, bool canContine)
		{
			this.pco = pco;
			CanContine = canContine;
		}

		public async Task<ValidationResult> ProcessAsync(ScheduleContext input)
		{
			var result = new ValidationResult();

			var errors = await GetTeameId(pco, input);

			result.AddErrors(errors);

			return result;
		}

		private async Task<List<ScheduleErrors>> GetTeameId(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var errors = new List<ScheduleErrors>();

			if (!scheduleContext.Configs.TryGetValue(PlanningCenterConstants.TeamConfigName, out CellValue<string> configValue))
			{
				var message = $"Could not find the {PlanningCenterConstants.TeamConfigName} config in the Config tab";
				errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					Message = message
				});
				return errors;
			}

			if (!configValue.HasValue)
			{
				var message = $"The {PlanningCenterConstants.TeamConfigName} has not been set in the Config tab";
				errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					Message = message
				});
				return errors;
			}

			var teamId = await pco.Services.GetTeamByName(scheduleContext.CachedManager.ServiceTypeId, configValue.Value);

			if (!teamId.data.Any())
			{
				var message = $"The {PlanningCenterConstants.TeamConfigName} called {scheduleContext.Configs[PlanningCenterConstants.TeamConfigName].Value} in the Config tab, does not exist on Planning Center";
				errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					CellCoordinate = scheduleContext.Configs[PlanningCenterConstants.TeamConfigName],
					Message = message
				});
				return errors;
			}

			scheduleContext.CachedManager.TeamId = teamId.data.First().id;

			return errors;
		}
	}
}
