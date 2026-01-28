namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Interface
{
	/// <summary>
	/// Model that hold collection of scheduling assignement data.
	/// </summary>
	public interface IScheduleAssignmentsModel
	{
		public List<IScheduleAssignmentModel> ScheduleAssignmentModel { get; set; }
		public void AddAssignment(IScheduleAssignmentModel scheduleAssignmentModel);
	}
}
