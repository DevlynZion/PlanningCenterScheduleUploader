using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleModel : IScheduleModel
	{
		public IScheduleConfigModel ScheduleConfigModel { get; set; }
		public IScheduleAssignmentsModel ScheduleAssignmentsModel { get; set; }
		public List<string> Errors { get; }
		public HashSet<ICellValue> CellsToChange { get; }

		public ScheduleModel(IScheduleConfigModel scheduleConfigModel, IScheduleAssignmentsModel scheduleAssignmentsMode) 
		{
			ScheduleConfigModel = scheduleConfigModel;
			ScheduleAssignmentsModel = scheduleAssignmentsMode;
			Errors = new List<string>();
			CellsToChange = new HashSet<ICellValue>();
		}
	}
}
