namespace PlanningCenterScheduleUploaderLib.Schedule.Implementation
{
	public class ScheduleCachedManager
	{
		public string ServiceTypeId { get; set; }
		public string TeamId { get; set; }

		private Dictionary<string, string> personIds;
		private Dictionary<DateOnly, string> planIds;
		private Dictionary<string, string> roleIds;

		public bool AnyPersons() => personIds.Any();
		public bool AnyPlans() => planIds.Any();
		public bool AnyRoles() => roleIds.Any();
		public Dictionary<string, string> GetPersons() => personIds;
		public Dictionary<DateOnly, string> GetPlans() => planIds;
		public Dictionary<string, string> GetRoles() => roleIds;

		public ScheduleCachedManager() 
		{
			ServiceTypeId = string.Empty;
			TeamId = string.Empty;
			personIds = new Dictionary<string, string>();
			planIds = new Dictionary<DateOnly, string>();
			roleIds = new Dictionary<string, string>();
		}

		public void AddPerson(string name, string id)
		{
			if(!personIds.ContainsKey(name))
				personIds.Add(name, id);
		}

		public void AddPlan(DateOnly date, string id)
		{
			if (!planIds.ContainsKey(date))
				planIds.Add(date, id);
		}

		public void AddRole(string name, string id)
		{
			if (!roleIds.ContainsKey(name))
				roleIds.Add(name, id);
		}

		public string GetPerson(string name)
		{
			if (personIds.ContainsKey(name))
				return personIds[name];
			else
				return string.Empty;
		}

		public string GetPlan(DateOnly date)
		{
			if (planIds.ContainsKey(date))
				return planIds[date];
			else
				return string.Empty;
		}

		public string GetRole(string name)
		{
			if (roleIds.ContainsKey(name))
				return roleIds[name];
			else
				return string.Empty;
		}
	}
}
