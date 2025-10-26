using PlanningCenterAPI.Type.Implementation;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IPeople : ICall
	{
		public Task<PeoplesResponse> GetPeople();
	}
}
