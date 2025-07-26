using PlanningCenterAPI.Type;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface IServices
	{
		public Task<Root> GetService_types();
	}
}
