using PlanningCenterAPI.Respone.Constant;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IServices : ICall
	{
		public Task<GetServiceTypesResponse.Rootobject> GetServiceTypes();
		public Task<GetServiceTypesRespone.Rootobject> GetServiceTypesByName(string serviceTypeName);
		public Task<GetPlanTemplatesRespone.Rootobject> GetPlanTemplates(string serviceTypeId);
		public Task<GetTeamsResponse.Rootobject> GetTeams(string serviceTypeId);
		public Task<GetTeamByNameRespone.Rootobject> GetTeamByName(string serviceTypeId, string teamName);
		public Task<GetPeoplesByTeamIdRespone.Rootobject> GetPeoplesByTeamId(string teamId);
		public Task<GetTeamPositionsByTeamIdRespone.Rootobject> GetTeamPositionsByTeamId(string teamId);
		public Task<GetTeamPositionByServiceTypeIdTeamPositionsIdRespone.Rootobject> GetTeamPositionByServiceTypeIdTeamPositionsId(string serviceTypesId, string id);
		public Task<AddScheduleTeamMembersSpecialResponse.Rootobject> AddScheduleTeamMembersSpecial(string serivesTypeId, string planId, string teamId, string teamPositionName, string peopleId);
		public Task<AddScheduleTeamMembersResponse.Rootobject> AddScheduleTeamMembers(string serivesTypeId, string planId, string teamId, string teamPositionName, string peopleId);
		public Task<GetPlansResponse.Rootobject> GetPlans(string serviceTypeId);
		public Task<GetPersonByNameRespone.Rootobject> GetPersonByName(string fullName);
		public Task<GetPlanAssignmentsRespone.Rootobject> GetPlanAssignments(string serviceTypeId, string planId, string teamId);
		public Task DeletePlanAssignments(string serviceTypeId, string planId, string planAssignmentId);
	}
}
