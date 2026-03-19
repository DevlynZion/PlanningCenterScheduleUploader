using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;

namespace PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface
{
	public interface IPipelineStep<T>
	{
		bool CanContine { get; }
		Task<ValidationResult> ProcessAsync(T input);
	}
}
