using PlanningCenterAPI.Call.Core.Interface;
using PlanningCenterAPI.Call.Implementation.Service;
using PlanningCenterAPI.Core;
using PlanningCenterAPI.OAuth;

namespace PlanningCenterAPI
{
    /// <summary>
    /// Handle all API calls and returning of data from Planning Center.
    /// Supports both Personal Access Token and OAuth 2.0 authentication.
    /// </summary>
    public class PlanningCenter : IDisposable
    {
        public IPeople People { get; private set; }
        public IServices Services { get; private set; }

        private Client client;
        private RateLimiter rateLimiter;
        private bool disposedValue;

        /// <summary>
        /// Creates a PlanningCenter instance using Personal Access Token authentication.
        /// </summary>
        public PlanningCenter(bool captureRequest = false, bool captureRespone = false, bool captureEndPoint = false)
        {
            client = new Client();
            client.CaptureRequest = captureRequest;
            client.CaptureRespone = captureRespone;
            client.CaptureEndPoint = captureEndPoint;
            rateLimiter = new RateLimiter(client);

            People = new People(rateLimiter);
            Services = new Services(rateLimiter);
        }

        /// <summary>
        /// Creates a PlanningCenter instance using OAuth 2.0 Bearer token authentication.
        /// </summary>
        /// <param name="tokenProvider">Async function that returns a valid Bearer access token.</param>
        /// <param name="captureRequest">Capture raw request to file (debug).</param>
        /// <param name="captureRespone">Capture raw response to file (debug).</param>
        /// <param name="captureEndPoint">Capture endpoint URL to file (debug).</param>
        public PlanningCenter(
            Func<Task<string?>> tokenProvider,
            bool captureRequest = false,
            bool captureRespone = false,
            bool captureEndPoint = false)
        {
            client = Client.CreateWithOAuth(tokenProvider);
            client.CaptureRequest = captureRequest;
            client.CaptureRespone = captureRespone;
            client.CaptureEndPoint = captureEndPoint;
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
