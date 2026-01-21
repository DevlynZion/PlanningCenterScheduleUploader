using PlanningCenterAPI.Respone.Implementation.Part;

namespace PlanningCenterAPI.Respone.Core.Interface
{
	public interface IMeta
	{
		public int total_count { get; set; }
		public int count { get; set; }
		public string[] can_order_by { get; set; }
		public string[] can_query_by { get; set; }
		public string[] can_include { get; set; }
		public string[] can_filter { get; set; }
		public IdType parent { get; set; }
	}
}
