namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Interface
{
	public interface ICellCoordinate
	{
		public int RowNumber { get; set; }    // Excel row number (1-based usually)
		public int ColumnIndex { get; set; }   // Column index (0-based or 1-based)
	}
}
