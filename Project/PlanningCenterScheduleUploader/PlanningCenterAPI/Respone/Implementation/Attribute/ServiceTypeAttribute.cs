using PlanningCenterAPI.Respone.Core.Interface;

namespace PlanningCenterAPI.Respone.Implementation.Attribute
{
	public class ServiceTypeAttribute : IAttribute
	{
		public object archived_at { get; set; }
		public bool attachment_types_enabled { get; set; }
		public string background_check_permissions { get; set; }
		public string comment_permissions { get; set; }
		public DateTime created_at { get; set; }
		public object[] custom_item_types { get; set; }
		public object deleted_at { get; set; }
		public string frequency { get; set; }
		public string last_plan_from { get; set; }
		public string name { get; set; }
		public string permissions { get; set; }
		public bool scheduled_publish { get; set; }
		public int sequence { get; set; }
		public Standard_Item_Types[] standard_item_types { get; set; }
		public DateTime updated_at { get; set; }

		public class Standard_Item_Types
		{
			public string name { get; set; }
			public string color { get; set; }
		}
	}
}
