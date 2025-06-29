using PlanningCenterAPI.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace PlanningCenterAPI.Call
{
	public class Services
	{
		private const string Product = "/services/v2";

		private readonly HttpClient httpClient;

		public Services(HttpClient httpClient)
		{
			this.httpClient = httpClient;
		}

		public async Task<Root> GetPeople()
		{
			var response = await httpClient.GetAsync($"{Product}/people");
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<Root>();
		}
	}
}
