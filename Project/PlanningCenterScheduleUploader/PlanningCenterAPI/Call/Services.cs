using PlanningCenterAPI.Core.Base;
using PlanningCenterAPI.Core.Interface;
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
	public class Services : ProductBase
	{
		private const string Product = "/services/v2";

		public Services(HttpClient httpClient) : base(httpClient) 
		{
			ProductURL = Product;
		}

		public async Task<Root> GetPeople()
		{
			return await GetAsync($"/people");
		}
	}
}
