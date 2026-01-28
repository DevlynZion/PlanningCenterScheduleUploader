using ExcelDataReader;
using PlanningCenterScheduleUploaderLib.Process.Core.Interface;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using System.Data;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;

namespace PlanningCenterScheduleUploaderLib.Process.Implementation
{
	public class ExcelProcessor : ISourceProcessor
	{
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

				var Setup = result.Tables["Setup"];
				var Schedule = result.Tables["Schedule"];

				IScheduleConfigModel scheduleConfigModel = LoadConfig(Setup);
				IScheduleAssignmentsModel scheduleAssignmentsModel = LoadAssignmentsModel(Schedule);

				return new ScheduleModel(scheduleConfigModel, scheduleAssignmentsModel);
			}
		}

		private IScheduleConfigModel LoadConfig(DataTable setup)
		{
			IScheduleConfigModel scheduleConfigModel = new ScheduleConfigModel();

			foreach (DataRow setupRow in setup.Rows)
			{
				var configKey = setupRow[0] as string;
				var configVale = setupRow[1] as string;

				if(configKey != null)
					scheduleConfigModel.AddConfig(configKey, configVale);
			}

			return scheduleConfigModel;
		}

		private IScheduleAssignmentsModel LoadAssignmentsModel(DataTable schedule)
		{

			// TODO: This is messing and error promne much fix!
			IScheduleAssignmentsModel scheduleAssignmentsModel = new ScheduleAssignmentsModel();

			var isFirstRow = true;
			var dateColumnName = string.Empty;
			var roleColumn = new Dictionary<DataColumn, string>();
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

						roleColumn.Add(role, roleName);
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
							scheduleAssignment = new ScheduleAssignmentModel(date.ToString(DataFormat));
						}
						else
						{
							scheduleAssignment.AddPersonToRole(roleColumn[role], assignRow.Field<string>(role));
						}
					}

					scheduleAssignmentsModel.AddAssignment(scheduleAssignment);
				}
			}

			return scheduleAssignmentsModel;
		}
	}
}
