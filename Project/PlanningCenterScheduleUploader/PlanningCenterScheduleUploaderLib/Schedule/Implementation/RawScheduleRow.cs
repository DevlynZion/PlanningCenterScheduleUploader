using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class RawScheduleRow
	{
		public DateOnly Date { get; set; }
		public string Role { get; set; }
		public CellValue<string> PersonName { get; set; }

		public RawScheduleRow()
		{
			Date = DateOnly.MinValue;
			Role = string.Empty;
			PersonName = new CellValue<string>();
		}
	}
}
