namespace PlanningCenterAPI.Respone.Constant
{
	public class GetTeamPositionsByTeamIdRespone
	{
		public class Rootobject
		{
			public Data data { get; set; }
			public Included[] included { get; set; }
			public Meta meta { get; set; }
		}

		public class Data
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
			public object stage_variant { get; set; }
			public DateTime updated_at { get; set; }
			public int viewers_see { get; set; }
		}

		public class Relationships
		{
			public Service_Type service_type { get; set; }
			public Default_Responds_To default_responds_to { get; set; }
			public Service_Types service_types { get; set; }
			public Team_Positions team_positions { get; set; }
		}

		public class Service_Type
		{
			public Data1 data { get; set; }
		}

		public class Data1
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Default_Responds_To
		{
			public Data2 data { get; set; }
		}

		public class Data2
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Service_Types
		{
			public Datum[] data { get; set; }
		}

		public class Datum
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Team_Positions
		{
			public Links links { get; set; }
			public Datum1[] data { get; set; }
		}

		public class Links
		{
			public string related { get; set; }
		}

		public class Datum1
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Links1
		{
			public string people { get; set; }
			public string person_team_position_assignments { get; set; }
			public string service_types { get; set; }
			public string team_leaders { get; set; }
			public string team_positions { get; set; }
			public string self { get; set; }
		}

		public class Meta
		{
			public string[] can_include { get; set; }
			public Parent parent { get; set; }
		}

		public class Parent
		{
			public string id { get; set; }
			public string type { get; set; }
		}

		public class Included
		{
			public string type { get; set; }
			public string id { get; set; }
			public Attributes1 attributes { get; set; }
			public Relationships1 relationships { get; set; }
			public Links2 links { get; set; }
		}

		public class Attributes1
		{
			public string name { get; set; }
			public object[] negative_tag_groups { get; set; }
			public int? sequence { get; set; }
			public object[] tag_groups { get; set; }
			public object[] tags { get; set; }
		}

		public class Relationships1
		{
			public Team team { get; set; }
			public Attachment_Types attachment_types { get; set; }
			public Tags tags { get; set; }
		}

		public class Team
		{
			public Data3 data { get; set; }
		}

		public class Data3
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Attachment_Types
		{
			public object[] data { get; set; }
		}

		public class Tags
		{
			public object[] data { get; set; }
		}

		public class Links2
		{
			public string self { get; set; }
		}

	}
}
