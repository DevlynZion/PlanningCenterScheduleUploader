using PlanningCenterAPI.Type.Core.Interface;
using PlanningCenterAPI.Type.Part;
using PlanningCenterAPI.Type.TopLevel;

namespace PlanningCenterAPI.Type
{
	public abstract class RootSingleResponeBase<T> : IRootRespone<DataAttribute<T>> where T : IAttribute
	{
		public Links links { get; set; }
		public DataAttribute<T> data { get; set; }
		public List<Included> included { get; set; }
		public Meta meta { get; set; }
	}
}
