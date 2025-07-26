using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Call.Implementation.Request;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Type.Implementation;
using PlanningCenterAPI.Type.Old;

namespace PlanningCenterAPI.Call.Implementation.Service
{
    public class People : IPeople
	{
		private RateLimiter rateLimiter;

		internal People(RateLimiter rateLimiter)
		{
			this.rateLimiter = rateLimiter;
		}

		public async Task<PeopleResponse> GetPeople()
		{
			var request = new GetRequest<PeopleResponse>("/services/v2/people");

			return await rateLimiter.EnqueueAsync(request);
		}
	}
}
