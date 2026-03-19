using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class RawConfigRow
	{
		public CellValue<string> ConfigKey { get; set; }
		public CellValue<string> ConfigValue { get; set; }

		public RawConfigRow()
		{ 
			ConfigKey = new CellValue<string>();
			ConfigValue = new CellValue<string>();
		}
	}
}
