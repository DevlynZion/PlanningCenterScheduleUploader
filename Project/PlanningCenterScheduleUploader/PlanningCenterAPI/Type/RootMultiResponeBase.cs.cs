using PlanningCenterAPI.Core.Interface;
using PlanningCenterAPI.Type.Part;
using PlanningCenterAPI.Type.TopLevel;

namespace PlanningCenterAPI.Type
{
	public abstract class RootMultiResponeBase<T> : IRootRespone<List<DataAttribute<T>>> where T : IAttribute
	{
		public Links links { get; set; }
		public List<DataAttribute<T>> data { get; set; }
		public List<Included> included { get; set; }
		public Meta meta { get; set; }
	}
}
