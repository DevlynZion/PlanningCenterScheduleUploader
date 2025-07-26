namespace PlanningCenterAPI.Type
{
	public class Data
	{
		public string type { get; set; }
		public string id { get; set; }
		public Attributes attributes { get; set; }
		public Relationships relationships { get; set; }
		public Links links { get; set; }
	}
}
