using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class CellValue : ICellValue
	{
		public string Tab { get; set; }
		public int Row { get; set; }
		public int Colnum { get; set; }
		public string Value { get; set; }
		public XLColor ChangeColourTo { get; set; }

		public static implicit operator string(CellValue cellValue) 
		{
			return cellValue.Value; 
		}

		public override string ToString()
		{
			return Value;
		}
	}
}
