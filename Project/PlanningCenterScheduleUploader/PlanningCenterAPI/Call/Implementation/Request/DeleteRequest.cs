using PlanningCenterAPI.Core;
using PlanningCenterAPI.Core.Interface;

namespace PlanningCenterAPI.Call.Implementation.Request
{
	internal class DeleteRequest : IRequestWaitable<bool>
	{
		public TaskCompletionSource<bool> TaskProgress { get; private set; }

		private string endpoint;

		internal DeleteRequest(string endpoint)
		{
			TaskProgress = new TaskCompletionSource<bool>();
			this.endpoint = endpoint;
		}

		public async Task PerformRequest(Client client)
		{
			try
			{
				await client.Delete(endpoint);
				TaskProgress.SetResult(true);
			}
			catch (Exception ex)
			{
				TaskProgress.SetException(ex);
				throw;
			}
		}
	}
}
