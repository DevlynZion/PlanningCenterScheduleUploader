using PlanningCenterAPI.Call.Core.Base;
using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Request.Constant;
using PlanningCenterAPI.Respone.Constant;
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

		public async Task<TeamsResponse> GetTeamsByService_typeId(string id)
		{
			return await GetRequest<TeamsResponse>($"/services/v2/service_types/{id}/teams");
		}

		public async Task<TeamResponse> GetTeamsById(string id)
		{
			return await GetRequest<TeamResponse>($"/services/v2/teams/{id}");
		}

		public async Task<TeamPositionsResponse> GetTeamPositionsByService_typeId(string id)
		{
			return await GetRequest<TeamPositionsResponse>($"/services/v2/service_types/{id}/team_positions");
		}

		public async Task<PeoplesResponse> GetPeoplesByTeamID(string id)
		{
			return await GetRequest<PeoplesResponse>($"/services/v2/teams/{id}/people");
		}

		public async Task<TeamResponse> GetTeamPositionsByTeamID(string id)
		{
			return await GetRequest<TeamResponse>($"/services/v2/teams/{id}?include=team_positions");
		}

		public async Task<TeamResponse> GetTeamPositionByServiceTypeIdTeamPositionsId(string serviceTypesId, string id)
		{
			return await GetRequest<TeamResponse>($"/services/v2/service_types/{serviceTypesId}/team_positions/{id}");
		}

		public async Task<AddScheduleTeamMembersResponse.Rootobject> AddScheduleTeamMembers(string serivesTypeId, string planId, string teamId, string teamPositionName, string peopleId)
		{
			var content = new AddScheduleTeamMembersRequest.Rootobject();
			content.data.attributes.team_id = Convert.ToInt32(teamId);
			content.data.attributes.team_position_name = teamPositionName;
			content.data.attributes.people_ids = new string[] { peopleId };

			return await PostRequest<AddScheduleTeamMembersResponse.Rootobject, AddScheduleTeamMembersRequest.Rootobject>($"/~api/services/v2/service_types/{serivesTypeId}/plans/{planId}/schedule_team_members", content);
		}
	}
}
