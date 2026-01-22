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

		public async Task<ServicesResponse> GetServiceTypes()
		{
			return await GetRequest<ServicesResponse>("/services/v2/service_types");
		}

		public async Task<ServicesResponse> GetPlanTemplates(string serviceTypeId)
		{
			return await GetRequest<ServicesResponse>($"/services/v2/service_types/{serviceTypeId}/plan_templates");
		}

		public async Task<TeamsResponse> GetTeams(string serviceTypeId)
		{
			return await GetRequest<TeamsResponse>($"/services/v2/service_types/{serviceTypeId}/teams");
		}

		public async Task<PeoplesResponse> GetPeoplesByTeamId(string teamId)
		{
			return await GetRequest<PeoplesResponse>($"/services/v2/teams/{teamId}/people");
		}

		public async Task<TeamResponse> GetTeamPositionsByTeamID(string teamId)
		{
			return await GetRequest<TeamResponse>($"/services/v2/teams/{teamId}?include=team_positions");
		}

		public async Task<TeamResponse> GetTeamPositionByServiceTypeIdTeamPositionsId(string serviceTypesId, string teamPositionId)
		{
			return await GetRequest<TeamResponse>($"/services/v2/service_types/{serviceTypesId}/team_positions/{teamPositionId}");
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
