namespace PlanningCenterAPI.Core.Interface
{
	internal interface IRequest
	{
		public Task PerformRequest(Client client);
	}
}
