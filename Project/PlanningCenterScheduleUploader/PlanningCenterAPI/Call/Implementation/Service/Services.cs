using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Call.Implementation.Request;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Type;

namespace PlanningCenterAPI.Call.Implementation.Service
{
	public class Services : IServices
	{
		private RateLimiter rateLimiter;

		internal Services(RateLimiter rateLimiter)
		{
			this.rateLimiter = rateLimiter;
		}

		public async Task<Root> GetService_types()
		{
			var request = new GetRequest("/services/v2/service_types");

			return await rateLimiter.EnqueueAsync<Root>(request);
		}
	}
}
