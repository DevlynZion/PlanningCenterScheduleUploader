using PlanningCenterAPI.Call.Implementation.Request;
using PlanningCenterAPI.Core;

namespace PlanningCenterAPI.Call.Core.Base
{
	public abstract class CallBase
	{
		private RateLimiter rateLimiter;

		internal CallBase(RateLimiter rateLimiter)
		{
			this.rateLimiter = rateLimiter;
		}

		public async Task<T> GetRequest<T>(string endpoint)
		{
			var request = new GetRequest<T>(endpoint);

			return await rateLimiter.EnqueueAsync(request);
		}
	}
}
