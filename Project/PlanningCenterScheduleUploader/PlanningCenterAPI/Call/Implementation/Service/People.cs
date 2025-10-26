using PlanningCenterAPI.Call.Core.Base;
using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Type.Implementation;

namespace PlanningCenterAPI.Call.Implementation.Service
{
	public class People : CallBase, IPeople
	{
		internal People(RateLimiter rateLimiter) : base(rateLimiter) { }

		public async Task<PeoplesResponse> GetPeople()
		{
			return await GetRequest<PeoplesResponse>("/services/v2/people");
		}
	}
}
