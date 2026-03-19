using PlanningCenterAPI.Respone.Interface;

namespace PlanningCenterAPI.Respone.Constant
{
	public class GetPersonsAssingmentsResponse
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
			public bool can_accept_partial { get; set; }
			public DateTime created_at { get; set; }
			public object decline_reason { get; set; }
			public string name { get; set; }
			public object notes { get; set; }
			public object notification_changed_at { get; set; }
			public object notification_changed_by_name { get; set; }
			public object notification_prepared_at { get; set; }
			public object notification_read_at { get; set; }
			public string notification_sender_name { get; set; }
			public DateTime? notification_sent_at { get; set; }
			public string photo_thumbnail { get; set; }
			public bool prepare_notification { get; set; }
			public string status { get; set; }
			public object status_updated_at { get; set; }
			public string team_position_name { get; set; }
			public DateTime updated_at { get; set; }
		}

		public class Relationships
		{
			public Person person { get; set; }
			public Plan plan { get; set; }
			public Scheduled_By scheduled_by { get; set; }
			public Service_Type service_type { get; set; }
			public Team team { get; set; }
			public Responds_To responds_to { get; set; }
			public Times times { get; set; }
			public Service_Times service_times { get; set; }
		}

		public class Person
		{
			public Data data { get; set; }
		}

		public class Data
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Plan
		{
			public Data1 data { get; set; }
		}

		public class Data1
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Scheduled_By
		{
			public Data2 data { get; set; }
		}

		public class Data2
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Service_Type
		{
			public Data3 data { get; set; }
		}

		public class Data3
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Team
		{
			public Data4 data { get; set; }
		}

		public class Data4
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Responds_To
		{
			public Data5 data { get; set; }
		}

		public class Data5
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Times
		{
			public Datum1[] data { get; set; }
		}

		public class Datum1
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Service_Times
		{
			public Datum2[] data { get; set; }
		}

		public class Datum2
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Links1
		{
			public string self { get; set; }
			public string html { get; set; }
		}
	}
}
