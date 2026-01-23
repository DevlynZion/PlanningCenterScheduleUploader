using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Process.Core.Interface
{
	/// <summary>
	/// That processes source to create a <see cref="IScheduleModel"/>.
	/// </summary>
	public interface ISourceProcessor
	{
		public IScheduleModel CreateScheduleModel();
	}
}
