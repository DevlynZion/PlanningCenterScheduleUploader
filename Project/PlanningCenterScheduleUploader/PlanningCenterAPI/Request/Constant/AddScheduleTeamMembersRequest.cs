namespace PlanningCenterAPI.Request.Constant
{
	public class AddScheduleTeamMembersRequest
	{
		public class Rootobject
		{
			public Data data { get; set; }

			public Rootobject() 
			{
				data = new Data();
			}
		}

		public class Data
		{
			public Attributes attributes { get; set; }
			public Data()
			{
				attributes = new Attributes();
			}
		}

		public class Attributes
		{
			public int team_id { get; set; }
			public string team_position_name { get; set; }
			public string[] people_ids { get; set; }
		}

	}
}
