using PlanningCenterAPI.Type;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IPeople
	{
		public Task<Root> GetPeople();
	}
}
