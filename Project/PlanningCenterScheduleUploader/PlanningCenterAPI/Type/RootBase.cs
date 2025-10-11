using PlanningCenterAPI.Core.Interface;
using PlanningCenterAPI.Type.Part;
using PlanningCenterAPI.Type.TopLevel;

namespace PlanningCenterAPI.Type
{
	public abstract class RootBase<T> where T : IAttribute
	{
		public Links links { get; set; }
		public List<Data<T>> data { get; set; }
		public List<Included> included { get; set; }
		public Meta meta { get; set; }
	}
}