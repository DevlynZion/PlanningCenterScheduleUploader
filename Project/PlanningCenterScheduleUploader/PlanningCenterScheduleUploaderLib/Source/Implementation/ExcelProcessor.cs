using ClosedXML.Excel;
using ExcelDataReader;
using PlanningCenterScheduleUploaderLib.Process.Core.Interface;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using System.Data;

namespace PlanningCenterScheduleUploaderLib.Process.Implementation
{
	public class ExcelProcessor : ISourceProcessor
	{
		private const string SetupTabName = "Setup";
		private const string ScheduleTabName = "Schedule";
		private const string DateColumnName = "date";
		private const int ConfigKeyColumnIndex = 0;
		private const int ConfigValueColumnIndex = 1;

		private string excelFilePath;

		public ExcelProcessor(string excelFilePath)
		{
			this.excelFilePath = excelFilePath;
			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
		}

		public ScheduleContext CreateScheduleModel()
		{
			using (var stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
			using (var reader = ExcelReaderFactory.CreateReader(stream))
			{
				var result = reader.AsDataSet();

				var Setup = result.Tables[SetupTabName];
				var Schedule = result.Tables[ScheduleTabName];

				var rawConfigRows = LoadConfig(Setup);
				var rawScheduleRows = LoadAssignmentsModel(Schedule, out Dictionary<int, CellValue<string>> rawScheduleRoleRow, out List<CellValue<DateOnly>> rawScheduleDateRow);

				var ScheduleContextFactory = new ScheduleContextFactory();

				return ScheduleContextFactory.Create(rawConfigRows, rawScheduleRows, rawScheduleRoleRow, rawScheduleDateRow);
			}
		}

		public void ProcessErrors(ScheduleContext scheduleContext)
		{
			if(scheduleContext == null)
				return;

			using (var workbook = new XLWorkbook(excelFilePath))
			{
				foreach (var error in scheduleContext.Errors)
				{
					if (error.CellCoordinate == null)
						continue;

					var worksheet = workbook.Worksheet(error.CellCoordinate.TabName);

					var changeColourTo = error.ErrorLevel switch
					{
						ErrorLevel.Error => XLColor.Crimson,
						ErrorLevel.Warnning => XLColor.Yellow,
						ErrorLevel.Information => XLColor.SkyBlue,
						_ => XLColor.NoColor,
					};

					worksheet.Cell(error.CellCoordinate.RowNumber + 1, error.CellCoordinate.ColumnIndex + 1).Style.Fill.BackgroundColor = changeColourTo;
				}
				workbook.Save();
			}
		}

		private List<RawConfigRow> LoadConfig(DataTable setup)
		{
			List<RawConfigRow> rawConfigRows = new List<RawConfigRow>();

			foreach (DataRow setupRow in setup.Rows)
			{
				var configKey = setupRow.Field<string>(ConfigKeyColumnIndex);
				var configValue = setupRow.Field<string>(ConfigValueColumnIndex);
				var rowNumber = setup.Rows.IndexOf(setupRow);

				if (string.IsNullOrWhiteSpace(configKey))
					continue;

				var rawConfigRow = new RawConfigRow()
				{
					ConfigKey = new CellValue<string>()
					{
						TabName = SetupTabName,
						RowNumber = rowNumber,
						ColumnIndex = ConfigKeyColumnIndex,
						Value = configKey
					},
					ConfigValue = new CellValue<string>()
					{
						TabName = SetupTabName,
						RowNumber = rowNumber,
						ColumnIndex = ConfigValueColumnIndex,
						Value = configValue
					}
				};

				rawConfigRows.Add(rawConfigRow);
			}

			return rawConfigRows;
		}

		private List<RawScheduleRow> LoadAssignmentsModel(DataTable schedule, out Dictionary<int, CellValue<string>> rawScheduleRoleRow, out List<CellValue<DateOnly>> rawScheduleDateRow)
		{
            // TODO: This Phasing is making a lot of assumptions, we will look to refactor later.

            List<RawScheduleRow> rawScheduleRows = new List<RawScheduleRow>();

			var isFirstRow = true;
			var dateColumnName = string.Empty;
			rawScheduleRoleRow = new Dictionary<int, CellValue<string>>();
			rawScheduleDateRow = new List<CellValue<DateOnly>>();
			foreach (DataRow assignRow in schedule.Rows)
			{
				var rowNumber = schedule.Rows.IndexOf(assignRow);
				if (isFirstRow)
				{
					isFirstRow = false;
					foreach (DataColumn role in schedule.Columns)
					{
						var roleName = assignRow.Field<string>(role);

						if (string.IsNullOrWhiteSpace(roleName))
							continue;

						if (roleName.ToLowerInvariant() == DateColumnName)
						{
							dateColumnName = role.ColumnName;
							continue;
						}

						CellValue<string> newRole = new CellValue<string>()
						{
							TabName = ScheduleTabName,
							RowNumber = rowNumber,
							ColumnIndex = schedule.Columns.IndexOf(role),
							Value = roleName
						};

						rawScheduleRoleRow.Add(newRole.ColumnIndex, newRole);
					}
				}
				else
				{
					var date = DateOnly.MinValue;
					foreach (DataColumn role in schedule.Columns)
					{
						var columnIndex = schedule.Columns.IndexOf(role);
						if (role.ColumnName == dateColumnName)
						{
							date = DateOnly.FromDateTime(assignRow.Field<DateTime>(role));
							rawScheduleDateRow.Add(new CellValue<DateOnly>()
							{
								TabName = ScheduleTabName,
								RowNumber = rowNumber,
								ColumnIndex = columnIndex,
								Value = date
							});
						}
						else
						{
							var person = assignRow.Field<string>(role);

							if (!rawScheduleRoleRow.ContainsKey(columnIndex))
								continue; // Skipping because there is blank Role Header.

							var roleName = rawScheduleRoleRow[columnIndex];

							if (string.IsNullOrWhiteSpace(person))
								continue;

							var rawScheduleRow = new RawScheduleRow()
							{
								Date = date,
								Role = roleName.Value,
								PersonName = new CellValue<string>()
								{
									TabName = ScheduleTabName,
									RowNumber = rowNumber,
									ColumnIndex = columnIndex,
									Value = person
								}
							};
							rawScheduleRows.Add(rawScheduleRow);
						}
					}
				}
			}

			return rawScheduleRows;
		}
	}
}
