using PlanningCenterScheduleUploaderLib.Schedule.Implementation;

namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Interface
{
	/// <summary>
	/// Model that hold scheduling assignement data for a date.
	/// </summary>
	public interface IScheduleAssignmentModel
	{
		public CellValue Date { get; set; }
		public Dictionary<CellValue, CellValue> RolePersons { get; set; }

		public void AddPersonToRole(CellValue role, CellValue person);
	}
}
