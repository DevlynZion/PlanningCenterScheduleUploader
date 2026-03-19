using PlanningCenterAPI;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using PlanningCenterScheduleUploaderLib.Scheduler.Core.Constant;

namespace PlanningCenterScheduleUploaderLib.Validation.Implementation.PlanningCenterValidation
{
	public class ServiceTypeValidationStep : IPipelineStep<ScheduleContext>
	{
		public bool CanContine { get; }

		private PlanningCenter pco;

		public ServiceTypeValidationStep(PlanningCenter pco, bool canContine) 
		{
			this.pco = pco;
			CanContine = canContine;
		}

		public async Task<ValidationResult> ProcessAsync(ScheduleContext input)
		{
			var result = new ValidationResult();

			var errors = await GetServiceTypeId(input);

			result.AddErrors(errors);

			return result;
		}

		private async Task<List<ScheduleErrors>> GetServiceTypeId(ScheduleContext scheduleContext)
		{
			var errors = new List<ScheduleErrors>();

			if (!scheduleContext.Configs.TryGetValue(PlanningCenterConstants.ServiceTypeConfigName, out CellValue<string> configValue))
			{
				var message = $"Could not find the {PlanningCenterConstants.ServiceTypeConfigName} config in the Config tab";
				errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					Message = message
				});
				return errors;
			}

			if (!configValue.HasValue)
			{
				var message = $"The {PlanningCenterConstants.ServiceTypeConfigName} has not been set in the Config tab";
				errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					CellCoordinate = configValue,
					Message = message
				});
				return errors;
			}

			var serviceType = await pco.Services.GetServiceTypesByName(configValue.Value);

			if (!serviceType.data.Any())
			{
				var message = $"The {PlanningCenterConstants.ServiceTypeConfigName} called {scheduleContext.Configs[PlanningCenterConstants.ServiceTypeConfigName].Value} in the Config tab, does not exist on Planning Center";
				errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Error,
					CellCoordinate = configValue,
					Message = message
				});
				return errors;
			}

			scheduleContext.CachedManager.ServiceTypeId = serviceType.data.First().id;

			return errors;
		}
	}
}
