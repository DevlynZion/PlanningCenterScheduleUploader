using PlanningCenterScheduleUploaderLib.Schedule.Core.Record;

namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleContextFactory
	{
		public ScheduleContext Create(List<RawConfigRow> rawConfigRows, List<RawScheduleRow> rawScheduleRows, Dictionary<int, CellValue<string>> rawScheduleRoleRow, List<CellValue<DateTime>> rawScheduleDateRow)
		{
			var scheduleContext = new ScheduleContext();

			CreateScheduleConfigContainer(scheduleContext, rawConfigRows);
			CreateScheduleAssignment(scheduleContext, rawScheduleRows);
			CreateScheduleRole(scheduleContext, rawScheduleRoleRow);
			CreateScheduleDate(scheduleContext, rawScheduleDateRow);

			return scheduleContext;
		}

		private void CreateScheduleConfigContainer(ScheduleContext scheduleContext, List<RawConfigRow> rawConfigRows)
		{
			foreach (var row in rawConfigRows)
				scheduleContext.Configs.Add(row.ConfigKey.Value, row.ConfigValue);
		}
		private void CreateScheduleAssignment(ScheduleContext scheduleContext, List<RawScheduleRow> rawScheduleRows)
		{
			foreach (var row in rawScheduleRows)
			{
				if (!row.PersonName.HasValue)
					continue;

				scheduleContext.Assignments.Add(new ScheduleAssignment
				{
					Date = row.Date,
					Role = row.Role,
					PersonName = row.PersonName
				});
			}
		}

		private void CreateScheduleRole(ScheduleContext scheduleContext, Dictionary<int, CellValue<string>> rawScheduleRoleRow)
		{
			foreach (var row in rawScheduleRoleRow)
			{
				if (string.IsNullOrWhiteSpace(row.Value.Value))
					continue;

				scheduleContext.ScheduleRoles.Add(row.Value.Value, row.Value);
			}
		}

		private void CreateScheduleDate(ScheduleContext scheduleContext, List<CellValue<DateTime>> rawScheduleDateRow)
		{
			foreach (var row in rawScheduleDateRow)
				scheduleContext.ScheduleDates.Add(row);
		}
	}
}
