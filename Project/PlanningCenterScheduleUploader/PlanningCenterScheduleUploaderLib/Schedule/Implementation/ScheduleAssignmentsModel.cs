using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;
using System.Data;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleAssignmentsModel : IScheduleAssignmentsModel
	{
		public HashSet<string> Roles { get; set; }
		public List<IScheduleAssignmentModel> ScheduleAssignmentModel { get; set; }

		public ScheduleAssignmentsModel() 
		{
			Roles = new HashSet<string>();
			ScheduleAssignmentModel = new List<IScheduleAssignmentModel>();
		}

		public void AddRole(string role)
		{
			if(!Roles.Contains(role))
				Roles.Add(role);
		}

		public void AddAssignment(string date, IEnumerable<string> persons)
		{
			var scheduleAssignmentModel = new ScheduleAssignmentModel(date, persons);

			ScheduleAssignmentModel.Add(scheduleAssignmentModel);
		}
	}
}
