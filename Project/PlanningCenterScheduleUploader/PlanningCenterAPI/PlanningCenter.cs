using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Call.Implementation.Service;
using PlanningCenterAPI.Core;

namespace PlanningCenterAPI
{
	/// <summary>
	/// Handle all API call and returning of data from Planning Center
	/// </summary>
	public class PlanningCenter : IDisposable
	{
		public IPeople People { get; private set; }
		public IServices Services { get; private set; }

		private Client client;
		private RateLimiter rateLimiter;
		private bool disposedValue;

		public PlanningCenter(bool captureRespone = false)
		{
			client = new Client();
			client.CaptureRespone = captureRespone;
			rateLimiter = new RateLimiter(client);

			People = new People(rateLimiter);
			Services = new Services(rateLimiter);
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
