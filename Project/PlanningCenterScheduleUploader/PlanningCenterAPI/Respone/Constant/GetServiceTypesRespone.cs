
namespace PlanningCenterAPI.Respone.Constant
{
	public class GetServiceTypesRespone
	{
		public class Rootobject
		{
			public Links links { get; set; }
			public Datum[] data { get; set; }
			public object[] included { get; set; }
			public Meta meta { get; set; }
		}

		public class Links
		{
			public string self { get; set; }
		}

		public class Meta
		{
			public int total_count { get; set; }
			public int count { get; set; }
			public string[] can_order_by { get; set; }
			public string[] can_query_by { get; set; }
			public string[] can_include { get; set; }
			public string[] can_filter { get; set; }
			public Parent parent { get; set; }
		}

		public class Parent
		{
			public string id { get; set; }
			public string type { get; set; }
		}

		public class Datum
		{
			public string type { get; set; }
			public string id { get; set; }
			public Attributes attributes { get; set; }
			public Relationships relationships { get; set; }
			public Links1 links { get; set; }
		}

		public class Attributes
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
		}

		public class Standard_Item_Types
		{
			public string name { get; set; }
			public int index { get; set; }
			public string color { get; set; }
		}

		public class Relationships
		{
			public Parent1 parent { get; set; }
		}

		public class Parent1
		{
			public object data { get; set; }
		}

		public class Links1
		{
			public string self { get; set; }
		}
	}
}
