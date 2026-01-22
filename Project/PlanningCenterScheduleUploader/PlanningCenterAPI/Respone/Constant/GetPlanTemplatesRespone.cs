using PlanningCenterAPI.Respone.Interface;

namespace PlanningCenterAPI.Respone.Constant
{
	public class GetPlanTemplatesRespone
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
			public bool can_view_order { get; set; }
			public DateTime created_at { get; set; }
			public int item_count { get; set; }
			public bool multi_day { get; set; }
			public string name { get; set; }
			public int note_count { get; set; }
			public bool prefers_order_view { get; set; }
			public bool rehearsable { get; set; }
			public int team_count { get; set; }
			public DateTime updated_at { get; set; }
		}

		public class Relationships
		{
			public Service_Type service_type { get; set; }
			public Created_By created_by { get; set; }
			public Updated_By updated_by { get; set; }
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

		public class Created_By
		{
			public Data1 data { get; set; }
		}

		public class Data1
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Updated_By
		{
			public Data2 data { get; set; }
		}

		public class Data2
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
