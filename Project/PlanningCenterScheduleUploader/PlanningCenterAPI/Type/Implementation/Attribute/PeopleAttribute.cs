using PlanningCenterAPI.Core.Interface;

namespace PlanningCenterAPI.Type.Implementation.Attribute
{
	public class PeopleAttribute : IAttribute
	{
		public string archived_at { get; set; }
		public bool accounting_administrator { get; set; }
		public string anniversary { get; set; }
		public string avatar { get; set; }
		public string birthdate { get; set; }
		public bool can_create_forms { get; set; }
		public bool can_email_lists { get; set; }
		public bool child { get; set; }
		public string created_at { get; set; }
		public string demographic_avatar_url { get; set; }
		public string directory_status { get; set; }
		public string first_name { get; set; }
		public string gender { get; set; }
		public string given_name { get; set; }
		public string grade { get; set; }
		public string graduation_year { get; set; }
		public string inactivated_at { get; set; }
		public string last_name { get; set; }
		public string login_identifier { get; set; }
		public string medical_notes { get; set; }
		public string membership { get; set; }
		public string middle_name { get; set; }
		public string Name => $"{first_name} {last_name}";
		public string nickname { get; set; }
		public bool passed_background_check { get; set; }
		public string people_permissions { get; set; }
		public string remote_id { get; set; }
		// resource_permission_flags 
		public string school_type { get; set; }
		public bool site_administrator { get; set; }
		public string status { get; set; }
		public string updated_at { get; set; }
	}
}
