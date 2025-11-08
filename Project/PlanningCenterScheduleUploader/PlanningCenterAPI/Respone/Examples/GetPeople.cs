
namespace PlanningCenterAPI.Respone.Examples
{
	public class GetPeople
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
			public bool access_media_attachments { get; set; }
			public bool access_plan_attachments { get; set; }
			public bool access_song_attachments { get; set; }
			public object anniversary { get; set; }
			public bool archived { get; set; }
			public object archived_at { get; set; }
			public bool assigned_to_rehearsal_team { get; set; }
			public string birthdate { get; set; }
			public bool can_edit_all_people { get; set; }
			public bool can_view_all_people { get; set; }
			public DateTime created_at { get; set; }
			public object facebook_id { get; set; }
			public string first_name { get; set; }
			public string full_name { get; set; }
			public object given_name { get; set; }
			public string ical_code { get; set; }
			public string last_name { get; set; }
			public string legacy_id { get; set; }
			public DateTime logged_in_at { get; set; }
			public string max_permissions { get; set; }
			public string max_plan_permissions { get; set; }
			public string me_tab { get; set; }
			public string media_permissions { get; set; }
			public string media_tab { get; set; }
			public object middle_name { get; set; }
			public object name_prefix { get; set; }
			public object name_suffix { get; set; }
			public object nickname { get; set; }
			public object notes { get; set; }
			public string[] onboardings { get; set; }
			public bool passed_background_check { get; set; }
			public string people_tab { get; set; }
			public string permissions { get; set; }
			public string photo_thumbnail_url { get; set; }
			public string photo_url { get; set; }
			public string plans_tab { get; set; }
			public bool praise_charts_enabled { get; set; }
			public string preferred_app { get; set; }
			public object preferred_max_plans_per_day { get; set; }
			public object preferred_max_plans_per_month { get; set; }
			public bool site_administrator { get; set; }
			public string song_permissions { get; set; }
			public string songs_tab { get; set; }
			public string status { get; set; }
			public DateTime updated_at { get; set; }
		}

		public class Relationships
		{
			public Created_By created_by { get; set; }
			public Updated_By updated_by { get; set; }
			public Current_Folder current_folder { get; set; }
		}

		public class Created_By
		{
			public Data data { get; set; }
		}

		public class Data
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Updated_By
		{
			public Data1 data { get; set; }
		}

		public class Data1
		{
			public string type { get; set; }
			public string id { get; set; }
		}

		public class Current_Folder
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
			public string html { get; set; }
		}
	}

}