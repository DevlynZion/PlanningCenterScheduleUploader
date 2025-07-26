using PlanningCenterAPI.Core;
using PlanningCenterAPI.Core.Interface;
using PlanningCenterAPI.Type;

namespace PlanningCenterAPI.Call.Implementation.Request
{
	internal class GetRequest : IRequestWaitable<Root>
	{
		public TaskCompletionSource<Root> TaskProgress { get; private set; }

		private string endpoint;

		internal GetRequest(string endpoint)
		{
			TaskProgress = new TaskCompletionSource<Root>();
			this.endpoint = endpoint;
		}

		public async Task PerformRequest(Client client)
		{
			TaskProgress.TrySetResult(await client.Get<Root>(endpoint));
		}
	}
}
