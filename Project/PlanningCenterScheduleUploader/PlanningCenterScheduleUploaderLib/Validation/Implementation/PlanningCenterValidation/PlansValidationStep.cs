using PlanningCenterAPI;
using PlanningCenterAPI.Respone.Constant;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using PlanningCenterScheduleUploaderLib.Scheduler.Core.Constant;

namespace PlanningCenterScheduleUploaderLib.Validation.Implementation.PlanningCenterValidation
{
	public class PlansValidationStep : IPipelineStep<ScheduleContext>
	{
		public bool CanContine { get; }
		private PlanningCenter pco;

		public PlansValidationStep(PlanningCenter pco, bool canContine)
		{
			this.pco = pco;
			CanContine = canContine;
		}

		public async Task<ValidationResult> ProcessAsync(ScheduleContext input)
		{
			var result = new ValidationResult();

			var errors = await CheckPlans(pco, input);

			result.AddErrors(errors);

			return result;
		}

		private async Task<List<ScheduleErrors>> CheckPlans(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var errors = new List<ScheduleErrors>();

			var planIds = await GetPlans(pco, scheduleContext);

			foreach (var plan in scheduleContext.ScheduleDates)
			{
				if (!planIds.ContainsKey(plan.Value))
				{
					var message = $"The Date {plan.Value.ToString(PlanningCenterConstants.DataFormat)} in the Schedule tab, does not exist on Planning Center";
					errors.Add(new ScheduleErrors()
					{
						ErrorLevel = ErrorLevel.Warnning,
						CellCoordinate = plan,
						Message = message
					});
				}
			}

			return errors;
		}

		private async Task<Dictionary<DateTime, string>> GetPlans(PlanningCenter pco, ScheduleContext scheduleContext)
		{
			var results = await pco.Services.GetPlans(scheduleContext.CachedManager.ServiceTypeId);
			do
			{
				foreach (var result in results.data)
					scheduleContext.CachedManager.AddPlan(result.attributes.sort_date.Date, result.id);

				results = await pco.Services.GetNextRequest<GetPlansResponse.Rootobject>(results.links);
			} while (results != null);

			return scheduleContext.CachedManager.GetPlans();
		}
	}
}
