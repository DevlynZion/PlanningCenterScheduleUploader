namespace PlanningCenterAPI.Generate.Core.Interface
{
	public interface IRequest
	{
		public Task PerformRequest(Client client);
	}
}
