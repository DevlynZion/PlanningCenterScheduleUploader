namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Interface
{
	/// <summary>
	/// Model that hold scheduling data.
	/// </summary>
	public interface IScheduleModel
	{
		public IScheduleConfigModel ScheduleConfigModel { get; set; }
		public IScheduleAssignmentsModel ScheduleAssignmentsModel { get; set; }
		public List<string> Errors { get; }
		public HashSet<ICellValue> CellsToChange { get; }
	}
}
