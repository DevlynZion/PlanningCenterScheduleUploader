using PlanningCenterAPI.Generate.Core;
using PlanningCenterAPI.Generate.Core.Interface;

namespace PlanningCenterAPI.Generate.Implementation.Request
{
	public class GetRequest : IRequestWaitable<string>
	{
		public TaskCompletionSource<string> TaskProgress { get; private set; }

		private string endpoint;

		internal GetRequest(string endpoint)
		{
			TaskProgress = new TaskCompletionSource<string>();
			this.endpoint = endpoint;
		}

		public async Task PerformRequest(Client client)
		{
			try
			{
				var data = await client.Get(endpoint);
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
