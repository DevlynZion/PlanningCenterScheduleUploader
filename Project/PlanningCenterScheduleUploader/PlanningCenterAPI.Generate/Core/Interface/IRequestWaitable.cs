namespace PlanningCenterAPI.Generate.Core.Interface
{
	public interface IRequestWaitable<T> : IRequest
	{
		public TaskCompletionSource<T> TaskProgress { get; }
	}
}
