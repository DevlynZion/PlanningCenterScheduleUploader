using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Call.Implementation.Request;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.Respone.Interface;

namespace PlanningCenterAPI.Call.Core.Base
{
	public abstract class CallBase : ICall
	{
		private RateLimiter rateLimiter;

		internal CallBase(RateLimiter rateLimiter)
		{
			this.rateLimiter = rateLimiter;
		}

		public async Task<T> GetNextRequest<T>(ILink link) where T : class
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

		protected async Task<T> PostRequest<T, C>(string endpoint, C content)
		{
			var request = new PostRequest<T, C>(endpoint, content);

			return await rateLimiter.EnqueueAsync(request);
		}

		protected async Task DeleteRequest(string endpoint)
		{
			var request = new DeleteRequest(endpoint);

			await rateLimiter.EnqueueAsync(request);
		}
	}
}
