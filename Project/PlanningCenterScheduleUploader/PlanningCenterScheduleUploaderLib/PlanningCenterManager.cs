using PlanningCenterScheduleUploaderLib.Process.Core.Interface;
using PlanningCenterScheduleUploaderLib.Schedule.Core.Interface;
using PlanningCenterScheduleUploaderLib.Scheduler.Implementation;

namespace PlanningCenterScheduleUploaderLib
{
	public class PlanningCenterManager
	{
		private ISourceProcessor sourceProcessor;
		private PlanningCenterScheduler planningCenterScheduler;
		private IScheduleModel scheduleModel;

		public PlanningCenterManager(ISourceProcessor sourceProcessor)
		{
			this.sourceProcessor = sourceProcessor;
		}


		public async Task Start()
		{
			try
			{
				//1.Input Excel with scheduling data.
				scheduleModel = sourceProcessor.CreateScheduleModel();
				planningCenterScheduler = new PlanningCenterScheduler(scheduleModel);
				//2.Do Checks on Data with Planning Centre.
				await planningCenterScheduler.DoChecks();
				//3.Clear Plans for team on Planning Centre.
				await planningCenterScheduler.ClearPlans();
				//4.Mark in excel file all errors.
				//5.Submit assignments on Planning Centre.
			}
			catch(ArgumentException ex)
			{
				throw;
			}
			finally
			{
				sourceProcessor.ProcessErrors(scheduleModel);
			}
		}

		public List<string> GetErrorMessages()
		{
			return scheduleModel.Errors;
		}
	}
}
