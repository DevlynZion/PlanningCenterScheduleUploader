namespace PlanningCenterScheduleUploader
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Test();
			Console.ReadKey();
		}

		static async void Test()
		{
			var handler = new SocketsHttpHandler()
			{
				PooledConnectionLifetime = TimeSpan.FromMinutes(2)
			};

			using (HttpClient client = new HttpClient(handler))
			{
				var response = await client.GetAsync("https://api.planningcenteronline.com/services/v2/people");
				var responseString = await response.Content.ReadAsStringAsync();
				Console.WriteLine(responseString);
			}
		}
	}
}