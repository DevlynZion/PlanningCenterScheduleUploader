using PlanningCenterAPI.Respone.Core.Interface;
using PlanningCenterAPI.Respone.Implementation.Part;

namespace PlanningCenterAPI.Respone.Implementation.Relationship
{
	public class ServiceTypeRelationship : IRelationships
	{
		public IdType parent { get; set; }
	}
}
