using PlanningCenterAPI.Type;

namespace PlanningCenterAPI.Core.Interface
{
	public interface IRequest
	{
		public Task PerformRequest(Client client);
	}
}
