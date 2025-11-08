using PlanningCenterAPI.Respone.Core.Interface;
using PlanningCenterAPI.Respone.Implementation.Part;
using PlanningCenterAPI.Respone.Implementation.TopLevel;

namespace PlanningCenterAPI.Respone.Core.Base
{
	public abstract class RootSingleResponeBase<Attribute, AttributeRelationships, AttributeLinks> : IRootRespone<Links, DataItem<Attribute, AttributeRelationships, AttributeLinks>, Attribute>
		where Attribute : IAttribute
		where AttributeRelationships : IRelationships
		where AttributeLinks : ILinks
	{
		public Links links { get; set; }
		public DataItem<Attribute, AttributeRelationships, AttributeLinks> data { get; set; }
		public Meta meta { get; set; }
	}
}
