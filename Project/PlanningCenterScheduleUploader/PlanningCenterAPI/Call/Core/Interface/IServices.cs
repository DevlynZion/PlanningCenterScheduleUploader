using PlanningCenterAPI.Respone.Constant;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IServices : ICall
	{
		public Task<GetServiceTypesResponse.Rootobject> GetServiceTypes();
		public Task<GetPlanTemplatesRespone.Rootobject> GetPlanTemplates(string serviceTypeId);
		public Task<GetTeamsResponse.Rootobject> GetTeams(string serviceTypeId);
		public Task<GetPeoplesByTeamIdRespone.Rootobject> GetPeoplesByTeamId(string teamId);
		public Task<GetTeamPositionsByTeamIdRespone.Rootobject> GetTeamPositionsByTeamId(string teamId);
		public Task<GetTeamPositionByServiceTypeIdTeamPositionsIdRespone.Rootobject> GetTeamPositionByServiceTypeIdTeamPositionsId(string serviceTypesId, string id);
		public Task<AddScheduleTeamMembersResponse.Rootobject> AddScheduleTeamMembers(string serivesTypeId, string planId, string teamId, string teamPositionName, string peopleId);
	}
}
