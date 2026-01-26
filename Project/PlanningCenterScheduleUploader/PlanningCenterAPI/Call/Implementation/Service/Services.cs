using PlanningCenterAPI.Call.Core.Base;
using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Request.Constant;
using PlanningCenterAPI.Respone.Constant;

namespace PlanningCenterAPI.Call.Implementation.Service
{
	public class Services : CallBase, IServices
	{
		internal Services(RateLimiter rateLimiter) : base(rateLimiter) {}

		public async Task<GetServiceTypesResponse.Rootobject> GetServiceTypes()
		{
			return await GetRequest<GetServiceTypesResponse.Rootobject>("/services/v2/service_types");
		}

		public async Task<GetPlanTemplatesRespone.Rootobject> GetPlanTemplates(string serviceTypeId)
		{
			return await GetRequest<GetPlanTemplatesRespone.Rootobject>($"/services/v2/service_types/{serviceTypeId}/plan_templates");
		}

		public async Task<GetTeamsResponse.Rootobject> GetTeams(string serviceTypeId)
		{
			return await GetRequest<GetTeamsResponse.Rootobject>($"/services/v2/service_types/{serviceTypeId}/teams");
		}

		public async Task<GetPeoplesByTeamIdRespone.Rootobject> GetPeoplesByTeamId(string teamId)
		{
			return await GetRequest<GetPeoplesByTeamIdRespone.Rootobject>($"/services/v2/teams/{teamId}/people");
		}

		public async Task<GetTeamPositionsByTeamIdRespone.Rootobject> GetTeamPositionsByTeamId(string teamId)
		{
			return await GetRequest<GetTeamPositionsByTeamIdRespone.Rootobject>($"/services/v2/teams/{teamId}?include=team_positions");
		}

		public async Task<GetTeamPositionByServiceTypeIdTeamPositionsIdRespone.Rootobject> GetTeamPositionByServiceTypeIdTeamPositionsId(string serviceTypesId, string teamPositionId)
		{
			return await GetRequest<GetTeamPositionByServiceTypeIdTeamPositionsIdRespone.Rootobject>($"/services/v2/service_types/{serviceTypesId}/team_positions/{teamPositionId}");
		}

		public async Task<AddScheduleTeamMembersSpecialResponse.Rootobject> AddScheduleTeamMembersSpecial(string serivesTypeId, string planId, string teamId, string teamPositionName, string peopleId)
		{
			var content = new AddScheduleTeamMembersSpecialRequest.Rootobject();
			content.data.attributes.team_id = Convert.ToInt32(teamId);
			content.data.attributes.team_position_name = teamPositionName;
			content.data.attributes.people_ids = new string[] { peopleId };

			return await PostRequest<AddScheduleTeamMembersSpecialResponse.Rootobject, AddScheduleTeamMembersSpecialRequest.Rootobject>($"/~api/services/v2/service_types/{serivesTypeId}/plans/{planId}/schedule_team_members", content);
		}
		public async Task<AddScheduleTeamMembersResponse.Rootobject> AddScheduleTeamMembers(string serivesTypeId, string planId, string teamId, string teamPositionName, string peopleId)
		{
			// TODO: Possible to to multiple 
			var content = new AddScheduleTeamMembersRequest.Rootobject();
			content.data.attributes.person_id = peopleId;
			content.data.attributes.team_id = teamId;
			content.data.attributes.team_position_name = teamPositionName;

			return await PostRequest<AddScheduleTeamMembersResponse.Rootobject, AddScheduleTeamMembersRequest.Rootobject>($"/services/v2/service_types/{serivesTypeId}/plans/{planId}/team_members", content);
		}
	}
}
