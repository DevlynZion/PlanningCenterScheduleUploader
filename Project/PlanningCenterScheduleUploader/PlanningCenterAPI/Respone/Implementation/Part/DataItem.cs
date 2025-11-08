using PlanningCenterAPI.Respone.Core.Interface;

namespace PlanningCenterAPI.Respone.Implementation.Part
{
	public class DataItem<Attribute, AttributeRelationships, AttributeLinks> : IIdType
		where Attribute : IAttribute
		where AttributeRelationships : IRelationships
		where AttributeLinks : ILinks
	{
		public string id { get; set; }
		public string type { get; set; }
		public Attribute attributes { get; set; }
		public AttributeRelationships relationships { get; set; }
		public AttributeLinks links { get; set; }
	}
}
