using PlanningCenterAPI.Type.Implementation;
using System.Dynamic;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IServices : ICall
	{
		public Task<ServicesResponse> GetService_types();
		public Task<ServicesResponse> GetPlan_templatesByService_typeId(string id);
		public Task<TeamResponse> GetTeamsByService_typeId(string id);
		public Task<TeamResponse> GetTeamsById(string id);
		public Task<TeamPositionResponse> GetTeamPositionsByService_typeId(string id);
		public Task<PeopleResponse> GetPeoplesByTeamID(string id);
		public Task<TeamPositionResponse> GetTeamPositionsByTeamID(string id);
		public Task<ExpandoObject> GetTestByID(string id);
	}
}
