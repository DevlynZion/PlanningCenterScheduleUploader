using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using PlanningCenterScheduleUploaderLib.Process.Core.Interface;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CellValue = PlanningCenterScheduleUploaderLib.Schedule.Implementation.CellValue;

namespace PlanningCenterScheduleUploaderLib.Process.Implementation
{
	public class ExcelProcessor : ISourceProcessor
	{
		private const string SetupTabName = "Setup";
		private const string ScheduleTabName = "Schedule";
		private const string DateColumnName = "date";
		private const string DataFormat = "d MMM yyyy";
		private string excelFilePath;

		public ExcelProcessor(string excelFilePath)
		{
			this.excelFilePath = excelFilePath;
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
		}

		public IScheduleModel CreateScheduleModel()
		{
			using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
			using (var reader = ExcelReaderFactory.CreateReader(stream))
			{
				var result = reader.AsDataSet();

				var Setup = result.Tables[SetupTabName];
				var Schedule = result.Tables[ScheduleTabName];

				IScheduleConfigModel scheduleConfigModel = LoadConfig(Setup);
				IScheduleAssignmentsModel scheduleAssignmentsModel = LoadAssignmentsModel(Schedule);

				return new ScheduleModel(scheduleConfigModel, scheduleAssignmentsModel);
			}
		}

		public void ProcessErrors(IScheduleModel scheduleModel)
		{
			using (var workbook = new XLWorkbook(excelFilePath))
			{
				foreach (var cell in scheduleModel.CellsToChange)
				{
					var worksheet = workbook.Worksheet(cell.Tab);
					worksheet.Cell(cell.Row + 1, cell.Colnum + 1).Style.Fill.BackgroundColor = cell.ChangeColourTo;
				}
				workbook.Save();
			}
		}

		private IScheduleConfigModel LoadConfig(DataTable setup)
		{
			IScheduleConfigModel scheduleConfigModel = new ScheduleConfigModel();

			foreach (DataRow setupRow in setup.Rows)
			{
				var configKey = setupRow[0] as string;
				var configVale = setupRow[1] as string;

				if (configKey != null)
				{
					var configCellValue = new CellValue()
					{
						Tab = SetupTabName,
						Colnum = 1,
						Row = setup.Rows.IndexOf(setupRow),
						Value = configVale
					};
					scheduleConfigModel.AddConfig(configKey, configCellValue);
				}
			}

			return scheduleConfigModel;
		}

		private IScheduleAssignmentsModel LoadAssignmentsModel(DataTable schedule)
		{

			// TODO: This is messing and error promne much fix!
			IScheduleAssignmentsModel scheduleAssignmentsModel = new ScheduleAssignmentsModel();

			var isFirstRow = true;
			var dateColumnName = string.Empty;
			var roleColumn = new Dictionary<DataColumn, CellValue>();
			foreach (DataRow assignRow in schedule.Rows)
			{
				if (isFirstRow)
				{
					isFirstRow = false;
					foreach (DataColumn role in schedule.Columns)
					{
						var roleName = assignRow[role] as string;

						if (roleName.ToLowerInvariant() == DateColumnName)
						{
							dateColumnName = role.ColumnName;
							continue;
						}

						var roleCellValue = new CellValue()
						{
							Tab = ScheduleTabName,
							Colnum = schedule.Columns.IndexOf(role),
							Row = schedule.Rows.IndexOf(assignRow),
							Value = roleName
						};

						roleColumn.Add(role, roleCellValue);
					}
				}
				else
				{
					var date = DateTime.Now;
					IScheduleAssignmentModel scheduleAssignment = null;

					foreach (DataColumn role in schedule.Columns)
					{
						if (role.ColumnName == dateColumnName)
						{
							date = assignRow.Field<DateTime>(role);
							var dateCellValue = new CellValue()
							{
								Tab = ScheduleTabName,
								Colnum = schedule.Columns.IndexOf(role),
								Row = schedule.Rows.IndexOf(assignRow),
								Value = date.ToString(DataFormat)
							};
							scheduleAssignment = new ScheduleAssignmentModel(dateCellValue);
						}
						else
						{
							var dateCellValue = new CellValue()
							{
								Tab = ScheduleTabName,
								Colnum = schedule.Columns.IndexOf(role),
								Row = schedule.Rows.IndexOf(assignRow),
								Value = assignRow.Field<string>(role)
							};
							scheduleAssignment.AddPersonToRole(roleColumn[role], dateCellValue);
						}
					}

					scheduleAssignmentsModel.AddAssignment(scheduleAssignment);
				}
			}

			return scheduleAssignmentsModel;
		}

	}
}
