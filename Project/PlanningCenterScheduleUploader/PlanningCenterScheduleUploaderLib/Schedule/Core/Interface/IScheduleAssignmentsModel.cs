namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Interface
{
	/// <summary>
	/// Model that hold collection of scheduling assignement data.
	/// </summary>
	public interface IScheduleAssignmentsModel
	{
		public HashSet<string> Roles { get; set; }
		public List<IScheduleAssignmentModel> ScheduleAssignmentModel { get; set; }
		public void AddRole(string role);
		public void AddAssignment(string date, IEnumerable<string> persons);
	}
}
