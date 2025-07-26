using PlanningCenterAPI.Core;
using PlanningCenterAPI.Core.Interface;

namespace PlanningCenterAPI.Call.Implementation.Request
{
	internal class GetRequest<T> : IRequestWaitable<T>
	{
		public TaskCompletionSource<T> TaskProgress { get; private set; }

		private string endpoint;

		internal GetRequest(string endpoint)
		{
			TaskProgress = new TaskCompletionSource<T>();
			this.endpoint = endpoint;
		}

		public async Task PerformRequest(Client client)
		{
			try
			{
				var data = await client.Get<T>(endpoint);
				TaskProgress.SetResult(data);
			}
			catch (Exception ex)
			{
				TaskProgress.SetException(ex);
				throw;
			}
		}
	}
}
