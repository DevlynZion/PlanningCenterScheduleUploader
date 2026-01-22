using PlanningCenterAPI.Call.Core.Base;
using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Respone.Constant;

namespace PlanningCenterAPI.Call.Implementation.Service
{
	public class People : CallBase, IPeople
	{
		internal People(RateLimiter rateLimiter) : base(rateLimiter) { }

		public async Task<GetPeopleResponese.Rootobject> GetPeople()
		{
			return await GetRequest<GetPeopleResponese.Rootobject>("/services/v2/people");
		}
	}
}
