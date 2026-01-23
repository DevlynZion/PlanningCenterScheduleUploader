using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleAssignmentModel : IScheduleAssignmentModel
	{
		public string Date { get; set; }
		public List<string> Persons { get; set; }

		public ScheduleAssignmentModel(string date, IEnumerable<string> persons) 
		{
			Date = date;
			Persons = new List<string>(persons);
		}
	}
}
