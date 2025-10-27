using PlanningCenterAPI.Type.TopLevel;

namespace PlanningCenterAPI.Type.Core.Interface
{
	public interface IRootRespone<D> where D : class
	{
		public Links links { get; set; }
		public D data { get; set; }
		public List<Included> included { get; set; }
		public Meta meta { get; set; }
	}
}
