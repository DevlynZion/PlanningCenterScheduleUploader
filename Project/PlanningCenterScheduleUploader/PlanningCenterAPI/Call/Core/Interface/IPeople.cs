using PlanningCenterAPI.Type.Implementation;
using PlanningCenterAPI.Type.Old;

namespace PlanningCenterAPI.Call.Core.Interface
{
    public interface IPeople
	{
		public Task<PeopleResponse> GetPeople();
	}
}
