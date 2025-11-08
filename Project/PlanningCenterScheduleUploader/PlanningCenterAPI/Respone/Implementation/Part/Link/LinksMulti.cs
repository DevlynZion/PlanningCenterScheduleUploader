using PlanningCenterAPI.Respone.Core.Interface;

namespace PlanningCenterAPI.Respone.Implementation.Part.Link
{
	public class LinksMulti : ILinks
	{
		public string self { get; set; }
		public string prev { get; set; }
		public string next { get; set; }
	}
}
