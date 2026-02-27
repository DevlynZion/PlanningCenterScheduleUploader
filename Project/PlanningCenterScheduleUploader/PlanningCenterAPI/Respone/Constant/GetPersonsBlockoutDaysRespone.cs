using PlanningCenterAPI.Respone.Interface;

namespace PlanningCenterAPI.Respone.Constant
{
	public class GetPersonsBlockoutDaysRespone
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
			public DateTime created_at { get; set; }
			public string description { get; set; }
			public DateTime ends_at { get; set; }
			public string group_identifier { get; set; }
			public string organization_name { get; set; }
			public string reason { get; set; }
			public string repeat_frequency { get; set; }
			public object repeat_interval { get; set; }
			public object repeat_period { get; set; }
			public object repeat_until { get; set; }
			public object settings { get; set; }
			public bool share { get; set; }
			public DateTime starts_at { get; set; }
			public string time_zone { get; set; }
			public DateTime updated_at { get; set; }
		}

		public class Relationships
		{
			public Person person { get; set; }
			public Organization organization { get; set; }
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

		public class Organization
		{
			public Data1 data { get; set; }
		}

		public class Data1
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
