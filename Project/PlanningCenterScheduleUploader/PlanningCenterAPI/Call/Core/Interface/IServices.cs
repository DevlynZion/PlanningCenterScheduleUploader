using PlanningCenterAPI.Type.Implementation;
using PlanningCenterAPI.Type.Old;

namespace PlanningCenterAPI.Call.Core.Interface
{
    public interface IServices
	{
		public Task<ServicesResponse> GetService_types();
	}
}
