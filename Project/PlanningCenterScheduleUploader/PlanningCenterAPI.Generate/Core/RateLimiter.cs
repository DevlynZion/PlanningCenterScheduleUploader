using PlanningCenterAPI.Generate.Core.Interface;

namespace PlanningCenterAPI.Generate.Core
{
	/// <summary>
	/// Manages the Rate limits of the API calls
	/// </summary>
	public class RateLimiter : IDisposable
	{
		private const double RequestRatePeriod = 20d;
		private const int RequestRateLimit = 100;

		private readonly TimeSpan delay;
		private readonly Client client;

		private Queue<IRequest> requests; // TODO: Maybe use Channel for a thread safe way, if needed
		private Thread processRequestsThread;
		private bool isRunning;
		private bool disposedValue;

		public RateLimiter(Client client)
		{
			isRunning = true;
			delay = TimeSpan.FromSeconds(RequestRatePeriod / RequestRateLimit);
			this.client = client;
			requests = new Queue<IRequest>();
			processRequestsThread = new Thread(ProcessRequests);
			processRequestsThread.IsBackground = true;
			processRequestsThread.Start();
		}

		public async Task<T> EnqueueAsync<T>(IRequestWaitable<T> request)
		{
			requests.Enqueue(request);

			return await request.TaskProgress.Task;
		}

		private async void ProcessRequests()
		{
			while (isRunning)
			{
				if (!requests.Any())
					continue;

				var request = requests.Dequeue();
				_ = request.PerformRequest(client);
				await Task.Delay(delay);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					isRunning = false;
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}