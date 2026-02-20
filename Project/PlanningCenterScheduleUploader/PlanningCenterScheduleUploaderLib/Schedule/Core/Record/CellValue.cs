using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Record
{
	public record CellValue<T> : ICellCoordinate
	{
		public int RowNumber { get; set; }
		public int ColumnIndex { get; set; }
		public bool HasValue => Value != null;
		public T? Value { get; set; }
	}
}
