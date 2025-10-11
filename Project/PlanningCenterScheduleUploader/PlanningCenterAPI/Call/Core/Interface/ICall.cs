using PlanningCenterAPI.Core.Interface;
using PlanningCenterAPI.Type;
using PlanningCenterAPI.Type.TopLevel;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface ICall
	{
		public Task<T> GetNextRequest<T>(Links link) where T : class;
	}
}
