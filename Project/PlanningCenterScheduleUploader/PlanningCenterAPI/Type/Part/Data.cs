using PlanningCenterAPI.Type.Interface;
using PlanningCenterAPI.Type.TopLevel;

namespace PlanningCenterAPI.Type.Part
{
	public class Data<T> : IdType
	{
		public T attributes { get; set; }
		//public Relationships relationships { get; set; }
		public Links links { get; set; }
	}
}
