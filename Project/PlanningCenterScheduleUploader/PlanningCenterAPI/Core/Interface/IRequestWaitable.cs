namespace PlanningCenterAPI.Core.Interface
{
	internal interface IRequestWaitable<T> : IRequest
	{
		public TaskCompletionSource<T> TaskProgress { get; }
	}
}
