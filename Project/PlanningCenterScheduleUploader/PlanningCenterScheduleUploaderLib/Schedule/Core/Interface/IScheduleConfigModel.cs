using PlanningCenterScheduleUploaderLib.Schedule.Implementation;

namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Interface
{
	/// <summary>
	/// Model that hold scheduling config data.
	/// </summary>
	public interface IScheduleConfigModel
	{
		public Dictionary<string, CellValue> Config { get; set; }

		public void AddConfig(string key, CellValue value);
	}
}
