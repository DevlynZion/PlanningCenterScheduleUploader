using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleContext
	{
		public Dictionary<string, CellValue<string>> Configs { get; private set; }
		public List<ScheduleAssignment> Assignments { get; private set; }
		public Dictionary<string, CellValue<string>> ScheduleRoles { get; private set; }
		public List<CellValue<DateTime>> ScheduleDates { get; private set; }
		public Dictionary<string, List<PersonBlockDays>> PersonsBlockedDays { get; private set; }
		public ScheduleCachedManager CachedManager { get; private set; }
		public List<ScheduleErrors> Errors { get; private set; }

		public ScheduleContext()
		{
			Configs = new Dictionary<string, CellValue<string>>();
			Assignments = new List<ScheduleAssignment>();
			ScheduleRoles = new Dictionary<string, CellValue<string>>();
			ScheduleDates = new List<CellValue<DateTime>>();
			PersonsBlockedDays = new Dictionary<string, List<PersonBlockDays>>();
			CachedManager = new ScheduleCachedManager();
			Errors = new List<ScheduleErrors>();
		}
	}
}
