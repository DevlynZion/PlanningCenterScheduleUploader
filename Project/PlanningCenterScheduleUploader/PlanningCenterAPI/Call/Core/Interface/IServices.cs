using PlanningCenterAPI.Type.Implementation;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IServices
	{
		public Task<ServicesResponse> GetService_types();
		public Task<ServicesResponse> GetPlan_templatesByService_typeId(string id);
		public Task<TeamResponse> GetTeamsByService_typeId(string id);
	}
}
