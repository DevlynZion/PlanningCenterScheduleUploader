using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleConfigModel : IScheduleConfigModel
	{
		public Dictionary<string, CellValue> Config { get; set; }

		public ScheduleConfigModel() 
		{
			Config = new Dictionary<string, CellValue>();
		}

		public void AddConfig(string key, CellValue value)
		{
			if(!Config.ContainsKey(key))
			{
				Config.Add(key, value);
			}
		}
	}
}
