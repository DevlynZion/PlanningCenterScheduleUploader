namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Interface
{
	/// <summary>
	/// Model that hold scheduling assignement data for a date.
	/// </summary>
	public interface IScheduleAssignmentModel
	{
		public string Date { get; set; }
		public Dictionary<string, string> RolePersons { get; set; }

		public void AddPersonToRole(string role, string person);
	}
}
