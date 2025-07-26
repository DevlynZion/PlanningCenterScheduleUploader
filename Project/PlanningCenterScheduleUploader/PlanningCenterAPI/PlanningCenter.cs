using PlanningCenterAPI.Core;

namespace PlanningCenterAPI
{
	public class PlanningCenter : IDisposable
	{
		private Client client;
		private RateLimiter rateLimiter;
		private bool disposedValue;

		public PlanningCenter()
		{
			client = new Client();
			rateLimiter = new RateLimiter(client);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					rateLimiter.Dispose();
					client.Dispose();
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
