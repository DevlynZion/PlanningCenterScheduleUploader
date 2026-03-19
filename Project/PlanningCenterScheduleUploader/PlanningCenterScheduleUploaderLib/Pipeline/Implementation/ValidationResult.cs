using PlanningCenterScheduleUploaderLib.Schedule.Implementation;

namespace PlanningCenterScheduleUploaderLib.Pipeline.Implementation
{
	public class ValidationResult
	{
		public bool IsValid => !Errors.Any();
		public List<ScheduleErrors> Errors { get; }

		public ValidationResult()
		{
			Errors = new List<ScheduleErrors>();
		}

		public void AddErrors(List<ScheduleErrors> errors)
		{
			Errors.AddRange(errors);
		}
	}
}
