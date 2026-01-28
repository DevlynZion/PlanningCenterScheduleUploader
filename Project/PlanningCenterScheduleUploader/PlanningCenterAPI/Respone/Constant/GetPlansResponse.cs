using PlanningCenterAPI.Respone.Interface;

namespace PlanningCenterAPI.Respone.Constant
{
	public class GetPlansResponse
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
			public Next next { get; set; }
			public string[] can_order_by { get; set; }
			public string[] can_query_by { get; set; }
			public string[] can_include { get; set; }
			public string[] can_filter { get; set; }
			public Parent parent { get; set; }
		}

		public class Next
		{
			public int offset { get; set; }
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
			public string dates { get; set; }
			public DateTime files_expire_at { get; set; }
			public int items_count { get; set; }
			public DateTime last_time_at { get; set; }
			public bool multi_day { get; set; }
			public int needed_positions_count { get; set; }
			public int other_time_count { get; set; }
			public string permissions { get; set; }
			public int plan_notes_count { get; set; }
			public int plan_people_count { get; set; }
			public string planning_center_url { get; set; }
			public bool prefers_order_view { get; set; }
			public bool _public { get; set; }
			public bool rehearsable { get; set; }
			public int rehearsal_time_count { get; set; }
			public bool reminders_disabled { get; set; }
			public object series_title { get; set; }
			public int service_time_count { get; set; }
			public string short_dates { get; set; }
			public DateTime sort_date { get; set; }
			public object title { get; set; }
			public int total_length { get; set; }
			public DateTime updated_at { get; set; }
		}

		public class Relationships
		{
			public Service_Type service_type { get; set; }
			public Previous_Plan previous_plan { get; set; }
			public Next_Plan next_plan { get; set; }
			public Series series { get; set; }
			public Created_By created_by { get; set; }
			public Updated_By updated_by { get; set; }
			public Linked_Publishing_Episode linked_publishing_episode { get; set; }
			public Attachment_Types attachment_types { get; set; }
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

		public class Previous_Plan
		{
			public Data1 data { get; set; }
		}

		public class Data1
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Next_Plan
		{
			public Data2 data { get; set; }
		}

		public class Data2
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Series
		{
			public object data { get; set; }
		}

		public class Created_By
		{
			public Data3 data { get; set; }
		}

		public class Data3
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Updated_By
		{
			public Data4 data { get; set; }
		}

		public class Data4
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Linked_Publishing_Episode
		{
			public object data { get; set; }
		}

		public class Attachment_Types
		{
			public object[] data { get; set; }
		}

		public class Links1
		{
			public string self { get; set; }
			public string html { get; set; }
		}
	}
}
