using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public enum ErrorLevel
	{
		Information,
		Warnning,
		Error
	}

	public class ScheduleErrors
	{
		public ErrorLevel ErrorLevel { get; set; }
		public ICellCoordinate CellCoordinate { get; set; }
		public string Message { get; set; }
	}
}
