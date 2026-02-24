using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;

namespace PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface
{
	public interface IPipelineStep<T>
	{
		Task<ValidationResult> ProcessAsync(T input);
	}
}
