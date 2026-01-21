using PlanningCenterAPI.Respone.Implementation.Part;
using PlanningCenterAPI.Respone.Implementation.TopLevel;

namespace PlanningCenterAPI.Respone.Core.Interface
{
	public interface IRootRespone<L, D, A> 
		where L : ILinks 
		where D : class
		where A : IAttribute
	{
		public L links { get; set; }
		public D data { get; set; }

		// TODO: Will do later
		//public List<Included> included { get; set; } 
		public Meta meta { get; set; }
	}
}
