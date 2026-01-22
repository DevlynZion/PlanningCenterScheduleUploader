using PlanningCenterAPI.Respone.Constant;
using PlanningCenterAPI.Type.Implementation;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IServices : ICall
	{
		public Task<ServicesResponse> GetServiceTypes();
		public Task<ServicesResponse> GetPlanTemplates(string serviceTypeId);
		public Task<TeamsResponse> GetTeams(string serviceTypeId);
		public Task<PeoplesResponse> GetPeoplesByTeamId(string teamId);
		public Task<TeamResponse> GetTeamPositionsByTeamID(string teamId);
		public Task<TeamResponse> GetTeamPositionByServiceTypeIdTeamPositionsId(string serviceTypesId, string id);
		public Task<AddScheduleTeamMembersResponse.Rootobject> AddScheduleTeamMembers(string serivesTypeId, string planId, string teamId, string teamPositionName, string peopleId);
	}
}
