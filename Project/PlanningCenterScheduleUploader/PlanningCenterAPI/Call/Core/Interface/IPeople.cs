using PlanningCenterAPI.Type.Implementation;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IPeople
	{
		public Task<PeopleResponse> GetPeople();
	}
}
