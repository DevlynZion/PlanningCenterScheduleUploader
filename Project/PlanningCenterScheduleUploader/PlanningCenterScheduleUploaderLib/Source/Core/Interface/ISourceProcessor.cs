using PlanningCenterScheduleUploaderLib.Schedule.Implementation;

namespace PlanningCenterScheduleUploaderLib.Process.Core.Interface
{
	/// <summary>
	/// That processes source to create a <see cref="ScheduleContext"/>.
	/// </summary>
	public interface ISourceProcessor
	{
		public ScheduleContext CreateScheduleModel();
		public void ProcessErrors(ScheduleContext scheduleContext);
	}
}
