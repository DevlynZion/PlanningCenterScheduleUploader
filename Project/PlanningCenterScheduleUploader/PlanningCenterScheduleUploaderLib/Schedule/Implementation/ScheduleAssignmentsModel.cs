using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;
using System.Data;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleAssignmentsModel : IScheduleAssignmentsModel
	{
		public List<IScheduleAssignmentModel> ScheduleAssignmentModel { get; set; }

		public ScheduleAssignmentsModel() 
		{
			ScheduleAssignmentModel = new List<IScheduleAssignmentModel>();
		}

		public void AddAssignment(IScheduleAssignmentModel scheduleAssignmentModel)
		{
			ScheduleAssignmentModel.Add(scheduleAssignmentModel);
		}
	}
}
