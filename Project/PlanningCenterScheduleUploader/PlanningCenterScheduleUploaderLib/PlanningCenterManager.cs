using PlanningCenterScheduleUploaderLib.Process.Core.Interface;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using PlanningCenterScheduleUploaderLib.Scheduler.Implementation;

namespace PlanningCenterScheduleUploaderLib
{
	public class PlanningCenterManager
	{
		private ISourceProcessor sourceProcessor;
		private PlanningCenterScheduler planningCenterScheduler;
		private ScheduleContext scheduleContext;

		public bool AnyErrors => scheduleContext != null ? scheduleContext.Errors.Any() : false;
		public List<ScheduleErrors> Errors => scheduleContext != null ? scheduleContext.Errors : new List<ScheduleErrors>();

		public PlanningCenterManager(ISourceProcessor sourceProcessor)
		{
			this.sourceProcessor = sourceProcessor;
		}


		public async Task Start()
		{
			try
			{
				//1.Input Excel with scheduling data.
				scheduleContext = sourceProcessor.CreateScheduleModel();
				planningCenterScheduler = new PlanningCenterScheduler(scheduleContext);
				//2.Do Checks on Data with Planning Centre.
				await planningCenterScheduler.DoChecks();
				//3.Clear Plans for team on Planning Centre.
				await planningCenterScheduler.ClearPlans();
				//4.Submit assignments on Planning Centre.
				await planningCenterScheduler.SubmitScheduling();
			}
			finally
			{
				//5.Mark in excel file all errors.
				if (scheduleContext != null)
					sourceProcessor.ProcessErrors(scheduleContext);
			}
		}
	}
}
