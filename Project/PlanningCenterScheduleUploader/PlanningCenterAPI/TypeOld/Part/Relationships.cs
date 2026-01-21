using PlanningCenterAPI.Type.Implementation.Attribute;

namespace PlanningCenterAPI.Type.Part
{
	public class Relationships
	{
		public Parent<object> parent { get; set; }

		public RelationshipSingleType<ObjectAttribute> created_by { get; set; }
		public RelationshipSingleType<ObjectAttribute> updated_by { get; set; }
		public RelationshipSingleType<ObjectAttribute> current_folder { get; set; }

		public RelationshipSingleType<ServiceAttribute> service_type { get; set; }
		public RelationshipSingleType<PeopleAttribute> default_responds_to { get; set; }

		public RelationshipMultiType<ServiceAttribute> service_types { get; set; }
		public RelationshipMultiType<TeamAttibute> team_positions { get; set; }
	}
}