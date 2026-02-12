using ClosedXML.Excel;

namespace PlanningCenterScheduleUploaderLib.Schedule.Core.Interface
{
	public interface ICellValue
	{
		public string Tab { get; set; }
		public int Row { get; set; }
		public int Colnum { get; set; }
		public string Value { get; set; }
		public XLColor ChangeColourTo { get; set; }
	}
}
