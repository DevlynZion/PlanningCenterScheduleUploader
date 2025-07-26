using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Call.Implementation.Request;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Type.Implementation;
using PlanningCenterAPI.Type.Old;

namespace PlanningCenterAPI.Call.Implementation.Service
{
    public class Services : IServices
	{
		private RateLimiter rateLimiter;

		internal Services(RateLimiter rateLimiter)
		{
			this.rateLimiter = rateLimiter;
		}

		public async Task<ServicesResponse> GetService_types()
		{
			var request = new GetRequest<ServicesResponse>("/services/v2/service_types");

			return await rateLimiter.EnqueueAsync(request);
		}
	}
}
