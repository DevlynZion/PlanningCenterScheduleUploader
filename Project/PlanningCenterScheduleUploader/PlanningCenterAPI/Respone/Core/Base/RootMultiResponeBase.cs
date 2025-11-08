using PlanningCenterAPI.Respone.Core.Interface;
using PlanningCenterAPI.Respone.Implementation.Part;
using PlanningCenterAPI.Respone.Implementation.Part.Link;
using PlanningCenterAPI.Respone.Implementation.TopLevel;

namespace PlanningCenterAPI.Respone.Core.Base
{
	public abstract class RootMultiResponeBase<Attribute, AttributeRelationships, AttributeLinks> : IRootRespone<LinksMulti, List<DataItem<Attribute, AttributeRelationships, AttributeLinks>>, Attribute>
		where Attribute : IAttribute
		where AttributeRelationships : IRelationships
		where AttributeLinks : ILinks
	{
		public LinksMulti links { get; set; }
		public List<DataItem<Attribute, AttributeRelationships, AttributeLinks>> data { get; set; }
		public Meta meta { get; set; }
	}
}
