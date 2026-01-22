using PlanningCenterAPI.Respone.Constant;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IPeople : ICall
	{
		public Task<GetPeopleResponese.Rootobject> GetPeople();
	}
}
