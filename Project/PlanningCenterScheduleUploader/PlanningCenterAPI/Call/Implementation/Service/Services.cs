using PlanningCenterAPI.Call.Core.Base;
using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Type.Implementation;
using System.Dynamic;

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

		public async Task<TeamResponse> GetTeamsById(string id)
		{
			return await GetRequest<TeamResponse>($"/services/v2/teams/{id}");
		}

		public async Task<TeamPositionResponse> GetTeamPositionsByService_typeId(string id)
		{
			return await GetRequest<TeamPositionResponse>($"/services/v2/service_types/{id}/team_positions");
		}

		public async Task<PeopleResponse> GetPeoplesByTeamID(string id)
		{
			return await GetRequest<PeopleResponse>($"/services/v2/teams/{id}/people");
		}

		public async Task<TeamPositionResponse> GetTeamPositionsByTeamID(string id)
		{
			return await GetRequest<TeamPositionResponse>($"/services/v2/teams/{id}?include=team_positions");
		}

		public async Task<ExpandoObject> GetTestByID(string id)
		{
			return await GetRequest<ExpandoObject>($"/services/v2/teams/{id}");
		}
	}
}
