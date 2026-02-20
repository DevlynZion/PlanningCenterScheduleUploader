using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class RawScheduleRow
	{
		public DateTime Date { get; set; }
		public string Role { get; set; }
		public CellValue<string> PersonName { get; set; }

		public RawScheduleRow()
		{
			Date = DateTime.MinValue;
			Role = string.Empty;
			PersonName = new CellValue<string>();
		}
	}
}
