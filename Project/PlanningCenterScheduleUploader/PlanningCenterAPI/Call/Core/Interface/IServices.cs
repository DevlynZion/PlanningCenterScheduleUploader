using PlanningCenterAPI.Type.Implementation;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IServices : ICall
	{
		public Task<ServicesResponse> GetService_types();
		public Task<ServicesResponse> GetPlan_templatesByService_typeId(string id);
		public Task<TeamsResponse> GetTeamsByService_typeId(string id);
		public Task<TeamResponse> GetTeamsById(string id);
		public Task<TeamPositionResponse> GetTeamPositionsByService_typeId(string id);
		public Task<PeoplesResponse> GetPeoplesByTeamID(string id);
		public Task<TeamResponse> GetTeamPositionsByTeamID(string id);
	}
}
