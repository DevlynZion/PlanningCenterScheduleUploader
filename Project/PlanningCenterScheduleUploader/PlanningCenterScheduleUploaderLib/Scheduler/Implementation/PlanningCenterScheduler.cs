using PlanningCenterAPI;
using PlanningCenterScheduleUploaderLib.Pipeline.Core.Interface;
using PlanningCenterScheduleUploaderLib.Pipeline.Implementation;
using PlanningCenterScheduleUploaderLib.Schedule.Implementation;
using PlanningCenterScheduleUploaderLib.Validation.Implementation.PlanningCenterValidation;

namespace PlanningCenterScheduleUploaderLib.Scheduler.Implementation
{
	/// <summary>
	/// Submits the the scheduling data tp planning centure.
	/// </summary>
	public class PlanningCenterScheduler
	{
		private ScheduleContext scheduleContext;

		public PlanningCenterScheduler(ScheduleContext scheduleContext)
		{
			this.scheduleContext = scheduleContext;
		}

		public async Task DoChecks()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				var scheduleContextPipeline = CreateScheduleContextPipeline(pco);

				var result = await scheduleContextPipeline.Execute(scheduleContext);

				if (!result.IsValid)
					scheduleContext.Errors.AddRange(result.Errors);
			}
		}

		public async Task ClearPlans()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				foreach (var plan in scheduleContext.ScheduleDates)
				{
					var results = await pco.Services.GetPlanAssignments(scheduleContext.CachedManager.ServiceTypeId, scheduleContext.CachedManager.GetPlan(plan.Value), scheduleContext.CachedManager.TeamId);

					foreach (var result in results.data)
						await pco.Services.DeletePlanAssignments(scheduleContext.CachedManager.ServiceTypeId, scheduleContext.CachedManager.GetPlan(plan.Value), result.id);
				}
			}
		}

		public async Task SubmitScheduling()
		{
			using (PlanningCenter pco = new PlanningCenter())
			{
				foreach (var assignment in scheduleContext.Assignments)
				{
					if (string.IsNullOrWhiteSpace(assignment.PersonName.Value))
						continue;

					var planId = scheduleContext.CachedManager.GetPlan(assignment.Date);
					await pco.Services.AddScheduleTeamMembers(scheduleContext.CachedManager.ServiceTypeId, planId, scheduleContext.CachedManager.TeamId, assignment.Role, scheduleContext.CachedManager.GetPerson(assignment.PersonName.Value));
				}
			}
		}

		private Pipeline<ScheduleContext> CreateScheduleContextPipeline(PlanningCenter pco)
		{
			var scheduleContextSteps = new IPipelineStep<ScheduleContext>[]
			{
				// 2.1 Does Service Type exist on Planning Centre.
				new ServiceTypeValidationStep(pco, false),
				// 2.2 Does Team exist on Planning Centre.
				new TeamValidationStep(pco, false),
				// 2.3 Does Plans exist on Planning Centre.
				new PlansValidationStep(pco, true),
				// 2.4 Does Roles exist on Planning Centre.
				new RoleValidationStep(pco, true),
				// 2.5 Does the people exist on Planning Centre.
				new PeopleValidationStep(pco, true),
				// 2.6 Does the people exist in their assign roles on Planning Centre(Not sure if needed).
				// 2.7 Check for person blockouts days.
				new PersonsBlockedOutDaysValidationStep(pco, true)
				// 2.8 Check if person is assigned elsewhere.
			};

			return new Pipeline<ScheduleContext>(scheduleContextSteps);
		}
	}
}
