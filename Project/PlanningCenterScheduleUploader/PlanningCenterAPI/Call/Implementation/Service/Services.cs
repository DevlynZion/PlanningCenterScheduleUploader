using PlanningCenterAPI.Call.Core.Base;
using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Type.Implementation;

namespace PlanningCenterAPI.Call.Implementation.Service
{
	public class Services : CallBase, IServices
	{
		internal Services(RateLimiter rateLimiter) : base(rateLimiter) {}

		public async Task<ServicesResponse> GetService_types()
		{
			return await GetRequest<ServicesResponse>("/services/v2/service_types");
		}

		public async Task<ServicesResponse> GetPlan_templatesByService_typeId(string id)
		{
			return await GetRequest<ServicesResponse>($"/services/v2/service_types/{id}/plan_templates");
		}

		public async Task<TeamResponse> GetTeamsByService_typeId(string id)
		{
			return await GetRequest<TeamResponse>($"/services/v2/service_types/{id}/teams");
		}

		public async Task<TeamPositionResponse> GetTeamPositionsByService_typeId(string id)
		{
			return await GetRequest<TeamPositionResponse>($"/services/v2/service_types/{id}/team_positions");
		}
	}
}
