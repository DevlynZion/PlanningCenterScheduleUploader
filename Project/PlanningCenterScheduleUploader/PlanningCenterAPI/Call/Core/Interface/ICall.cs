using PlanningCenterAPI.Respone.Interface;

namespace PlanningCenterAPI.Call.Core.Interface
{
	public interface ICall
	{
		public Task<T> GetNextRequest<T>(ILink link) where T : class;
	}
}
