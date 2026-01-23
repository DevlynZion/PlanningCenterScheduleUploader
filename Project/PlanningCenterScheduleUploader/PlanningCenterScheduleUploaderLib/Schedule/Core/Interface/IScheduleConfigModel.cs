namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Interface
{
	/// <summary>
	/// Model that hold scheduling config data.
	/// </summary>
	public interface IScheduleConfigModel
	{
		public Dictionary<string, string> Config { get; set; }

		public void AddConfig(string key, string value);
	}
}
