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
        private const string ErrorCopy = " (Errors)";

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

				var loadErrors = new List<ScheduleErrors>();
				var rawConfigRows = LoadConfig(Setup);
				var rawScheduleRows = LoadAssignmentsModel(Schedule, loadErrors, out Dictionary<int, CellValue<string>> rawScheduleRoleRow, out List<CellValue<DateOnly>> rawScheduleDateRow);

				var ScheduleContextFactory = new ScheduleContextFactory();

				var scheduleContext = ScheduleContextFactory.Create(rawConfigRows, rawScheduleRows, rawScheduleRoleRow, rawScheduleDateRow);
				scheduleContext.Errors.AddRange(loadErrors);

				return scheduleContext;
			}
		}

		public void ProcessErrors(ScheduleContext scheduleContext)
		{
			if(scheduleContext == null || !scheduleContext.Errors.Any())
				return;

			var fileName = Path.GetFileNameWithoutExtension(excelFilePath);
			var fileExtension = Path.GetExtension(excelFilePath);
			var newExcelFilePath = Directory.GetCurrentDirectory() + "\\" + fileName + ErrorCopy + fileExtension;

			try
			{
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

					ClearUnsaveableDates(workbook);

					workbook.SaveAs(newExcelFilePath);
				}
			}
			catch (Exception ex)
			{
				// SaveAs can abort mid-stream and leave a truncated workbook behind; delete it so
				// the user never opens a corrupted copy. The error list still tells them where to look.
				try
				{
					if (File.Exists(newExcelFilePath))
						File.Delete(newExcelFilePath);
				}
				catch
				{
				}

				scheduleContext.Errors.Add(new ScheduleErrors()
				{
					ErrorLevel = ErrorLevel.Warnning,
					Message = $"Could not save a highlighted copy of the Excel file: {ex.Message}"
				});
			}
		}

		private List<RawConfigRow> LoadConfig(DataTable setup)
		{
			List<RawConfigRow> rawConfigRows = new List<RawConfigRow>();

			foreach (DataRow setupRow in setup.Rows)
			{
				var configKey = ReadCellAsString(setupRow, ConfigKeyColumnIndex);
				var configValue = ReadCellAsString(setupRow, ConfigValueColumnIndex);
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

		private List<RawScheduleRow> LoadAssignmentsModel(DataTable schedule, List<ScheduleErrors> loadErrors, out Dictionary<int, CellValue<string>> rawScheduleRoleRow, out List<CellValue<DateOnly>> rawScheduleDateRow)
		{
            // TODO: This Phasing is making a lot of assumptions, we will look to refactor later.

            List<RawScheduleRow> rawScheduleRows = new List<RawScheduleRow>();

			var isFirstRow = true;
			var dateColumnIndex = -1;
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
						var columnIndex = schedule.Columns.IndexOf(role);
						var roleName = ReadCellAsString(assignRow, columnIndex);

						if (string.IsNullOrWhiteSpace(roleName))
							continue;

						if (roleName.ToLowerInvariant() == DateColumnName)
						{
							dateColumnIndex = columnIndex;
							continue;
						}

						CellValue<string> newRole = new CellValue<string>()
						{
							TabName = ScheduleTabName,
							RowNumber = rowNumber,
							ColumnIndex = columnIndex,
							Value = roleName
						};

						rawScheduleRoleRow.Add(newRole.ColumnIndex, newRole);
					}
				}
				else
				{
					var date = DateOnly.MinValue;
					if (dateColumnIndex >= 0)
					{
						// Read the date first: without a usable date the row's assignments cannot be scheduled.
						var rawDate = assignRow[dateColumnIndex];
						if (rawDate is DateTime dateTime)
						{
							date = DateOnly.FromDateTime(dateTime);
							rawScheduleDateRow.Add(new CellValue<DateOnly>()
							{
								TabName = ScheduleTabName,
								RowNumber = rowNumber,
								ColumnIndex = dateColumnIndex,
								Value = date
							});
						}
						else if (IsRowEmpty(assignRow))
						{
							continue; // Skipping because the whole row is blank.
						}
						else
						{
							var message = rawDate == DBNull.Value
								? "The date is missing for this row in the Schedule tab"
								: $"The value {rawDate} in the date column of the Schedule tab is not a valid date";

							loadErrors.Add(new ScheduleErrors()
							{
								ErrorLevel = ErrorLevel.Error,
								CellCoordinate = new CellValue<string>()
								{
									TabName = ScheduleTabName,
									RowNumber = rowNumber,
									ColumnIndex = dateColumnIndex
								},
								Message = message
							});

							continue; // Skipping because the row's assignments have no usable date.
						}
					}

					foreach (DataColumn role in schedule.Columns)
					{
						var columnIndex = schedule.Columns.IndexOf(role);
						if (columnIndex == dateColumnIndex)
							continue;

						var person = ReadCellAsString(assignRow, columnIndex);

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

			return rawScheduleRows;
		}

		private static void ClearUnsaveableDates(XLWorkbook workbook)
		{
			// A date outside the range Excel can represent (e.g. year 20206 from a typo) loads
			// fine but makes ClosedXML's SaveAs throw and truncate the workbook mid-write.
			foreach (var worksheet in workbook.Worksheets)
			{
				foreach (var cell in worksheet.CellsUsed(c => c.DataType == XLDataType.DateTime))
				{
					try
					{
						cell.GetDateTime();
					}
					catch (ArgumentException)
					{
						cell.Clear(XLClearOptions.Contents);
					}
				}
			}
		}

		private static string? ReadCellAsString(DataRow row, int columnIndex)
		{
			var value = row[columnIndex];

			if (value == null || value == DBNull.Value)
				return null;

			return value.ToString();
		}

		private static bool IsRowEmpty(DataRow row)
		{
			return row.ItemArray.All(value => value == DBNull.Value || string.IsNullOrWhiteSpace(value?.ToString()));
		}
	}
}
