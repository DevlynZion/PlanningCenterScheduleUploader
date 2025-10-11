using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Call.Implementation.Request;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Core.Interface;
using PlanningCenterAPI.Type;
using PlanningCenterAPI.Type.TopLevel;

namespace PlanningCenterAPI.Call.Core.Base
{
	public abstract class CallBase : ICall
	{
		private RateLimiter rateLimiter;

		internal CallBase(RateLimiter rateLimiter)
		{
			this.rateLimiter = rateLimiter;
		}

		public async Task<T> GetNextRequest<T>(Links link) where T : class
		{
			if (string.IsNullOrEmpty(link.next))
				return null;

			return await GetRequest<T>(link.next);
		}

		protected async Task<T> GetRequest<T>(string endpoint)
		{
			var request = new GetRequest<T>(endpoint);

			return await rateLimiter.EnqueueAsync(request);
		}
	}
}
