using PlanningCenterAPI.Respone.Interface;

namespace PlanningCenterAPI.Respone.Constant
{
	public class GetTeamsResponse
	{
		public class Rootobject
		{
			public Links links { get; set; }
			public Datum[] data { get; set; }
			public object[] included { get; set; }
			public Meta meta { get; set; }
		}

		public class Links : ILink
		{
			public string self { get; set; }
			public string next { get; set; }
		}

		public class Meta
		{
			public int total_count { get; set; }
			public int count { get; set; }
			public string[] can_order_by { get; set; }
			public string[] can_query_by { get; set; }
			public string[] can_include { get; set; }
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
			public bool assigned_directly { get; set; }
			public DateTime created_at { get; set; }
			public bool default_prepare_notifications { get; set; }
			public string default_status { get; set; }
			public object deleted_at { get; set; }
			public string last_plan_from { get; set; }
			public string name { get; set; }
			public bool rehearsal_team { get; set; }
			public string schedule_to { get; set; }
			public bool secure_team { get; set; }
			public int sequence { get; set; }
			public string stage_color { get; set; }
			public string stage_variant { get; set; }
			public DateTime updated_at { get; set; }
			public int viewers_see { get; set; }
		}

		public class Relationships
		{
			public Service_Type service_type { get; set; }
			public Default_Responds_To default_responds_to { get; set; }
			public Service_Types service_types { get; set; }
		}

		public class Service_Type
		{
			public Data data { get; set; }
		}

		public class Data
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Default_Responds_To
		{
			public Data1 data { get; set; }
		}

		public class Data1
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Service_Types
		{
			public Datum1[] data { get; set; }
		}

		public class Datum1
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Links1
		{
			public string self { get; set; }
		}

	}
}
