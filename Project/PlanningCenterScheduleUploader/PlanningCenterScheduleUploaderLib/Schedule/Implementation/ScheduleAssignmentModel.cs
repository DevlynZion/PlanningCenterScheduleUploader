using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleAssignmentModel : IScheduleAssignmentModel
	{
		public string Date { get; set; }
		public Dictionary<string, string> RolePersons { get; set; }

		public ScheduleAssignmentModel(string date) 
		{
			Date = date;
			RolePersons = new Dictionary<string, string>();
		}

		public void AddPersonToRole(string role, string person)
		{
			if(!RolePersons.ContainsKey(role))
			{
				RolePersons.Add(role, person);
			}
		}
	}
}
