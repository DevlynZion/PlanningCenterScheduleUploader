using PlanningCenterAPI.Respone.Interface;

namespace PlanningCenterAPI.Respone.Constant
{
	public class GetTeamPositionByServiceTypeIdTeamPositionsIdRespone
	{
		public class Rootobject
		{
			public Data data { get; set; }
			public object[] included { get; set; }
			public Meta meta { get; set; }
		}

		public class Data
		{
			public string type { get; set; }
			public string id { get; set; }
			public Attributes attributes { get; set; }
			public Relationships relationships { get; set; }
			public Links links { get; set; }
		}

		public class Attributes
		{
			public string name { get; set; }
			public object[] negative_tag_groups { get; set; }
			public object sequence { get; set; }
			public object[] tag_groups { get; set; }
			public object[] tags { get; set; }
		}

		public class Relationships
		{
			public Team team { get; set; }
			public Attachment_Types attachment_types { get; set; }
			public Tags tags { get; set; }
		}

		public class Team
		{
			public Data1 data { get; set; }
		}

		public class Data1
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

		public class Links : ILink
		{
			public string person_team_position_assignments { get; set; }
			public string tags { get; set; }
			public string team { get; set; }
			public string self { get; set; }
			public string next { get; set; }
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

	}
}
