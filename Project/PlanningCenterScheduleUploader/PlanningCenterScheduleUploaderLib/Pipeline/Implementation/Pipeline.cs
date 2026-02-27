using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Pipeline.Implementation
{
	public class Pipeline<T>
	{
		private readonly IEnumerable<IPipelineStep<T>> steps;

		public Pipeline(IEnumerable<IPipelineStep<T>> steps)
		{
			this.steps = steps;
		}

		public async Task<ValidationResult> Execute(T context)
		{
			var finalResult = new ValidationResult();

			foreach (var step in steps)
			{
				var result = await step.ProcessAsync(context);

				if (!result.IsValid)
				{
					finalResult.AddErrors(result.Errors);
					if (!step.CanContine)
						return finalResult;
				}
			}

			return finalResult;
		}
	}
}
