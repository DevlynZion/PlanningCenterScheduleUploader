using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleConfigModel : IScheduleConfigModel
	{
		public Dictionary<string, string> Config { get; set; }

		public ScheduleConfigModel() 
		{
			Config = new Dictionary<string, string>();
		}

		public void AddConfig(string key, string value)
		{
			if(!Config.ContainsKey(key))
			{
				Config.Add(key, value);
			}
		}
	}
}
