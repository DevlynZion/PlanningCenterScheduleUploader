using PlanningCenterAPI.Type.Interface;

namespace PlanningCenterAPI.Type.TopLevel
{
	public class Meta 
	{
		public int total_count { get; set; }
		public int count { get; set; }
		//can_order_by[]
		//can_query_by[]
		//can_include[]
		//can_filter[]
		public IdType parent { get; set; }
	}
}