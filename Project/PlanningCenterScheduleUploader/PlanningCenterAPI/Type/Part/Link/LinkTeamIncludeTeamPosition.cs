using PlanningCenterAPI.Type.Core.Interface.Link;

namespace PlanningCenterAPI.Type.Part.Link
{
	public class LinkTeamIncludeTeamPosition : LinkSelf, ILinkPeople
	{
		public string people { get; set; }
	}
}
