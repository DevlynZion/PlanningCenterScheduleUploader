using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleAssignmentModel : IScheduleAssignmentModel
	{
		public CellValue Date { get; set; }
		public Dictionary<CellValue, CellValue> RolePersons { get; set; }

		public ScheduleAssignmentModel(CellValue date) 
		{
			Date = date;
			RolePersons = new Dictionary<CellValue, CellValue>();
		}

		public void AddPersonToRole(CellValue role, CellValue person)
		{
			if(!RolePersons.ContainsKey(role))
			{
				RolePersons.Add(role, person);
			}
		}
	}
}
