using PlanningCenterAPI.Type.Implementation;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IServices
	{
		public Task<ServicesResponse> GetService_types();
	}
}
