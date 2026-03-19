using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleAssignment
	{
		public DateOnly Date { get; set; }
		public string Role { get; set; }
		public CellValue<string> PersonName { get; set; }

		public ScheduleAssignment()
		{
			Date = DateOnly.MinValue;
			Role = string.Empty;
			PersonName = new CellValue<string>();
		}
	}
}
