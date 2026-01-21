using PlanningCenterAPI.Core;
using PlanningCenterAPI.Core.Interface;

namespace PlanningCenterAPI.Call.Implementation.Request
{
	internal class PostRequest<T, C> : IRequestWaitable<T>
	{
		public TaskCompletionSource<T> TaskProgress { get; private set; }

		private string endpoint;
		private C content;

		internal PostRequest(string endpoint, C content)
		{
			TaskProgress = new TaskCompletionSource<T>();
			this.endpoint = endpoint;
			this.content = content;
		}

		public async Task PerformRequest(Client client)
		{
			try
			{
				var data = await client.Post<T, C>(endpoint, content);
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
