using PlanningCenterAPI.Core.Interface;

namespace PlanningCenterAPI.Type.Implementation.Attribute
{
	public class TeamAttibute : IAttribute
	{
		public string archived_at { get; set; }
		public bool assigned_directly { get; set; }
		public string created_at { get; set; }
		public bool default_prepare_notifications { get; set; }
		public string default_status { get; set; }
		public string last_plan_from { get; set; }
		public string name { get; set; }
		public bool rehearsal_team { get; set; }
		public string schedule_to { get; set; }
		public bool secure_team { get; set; }
		public int? sequence { get; set; }
		public string stage_color { get; set; }
		public string stage_variant { get; set; }
		public string updated_at { get; set; }
		public int viewers_see { get; set; }
	}
}
