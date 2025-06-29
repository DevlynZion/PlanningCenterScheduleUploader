using PlanningCenterAPI.Core.Interface;
using PlanningCenterAPI.Type;
using System.Net.Http.Json;

namespace PlanningCenterAPI.Core.Base
{
	public abstract class ProductBase : IProduct
	{
		private readonly HttpClient httpClient;

		protected string ProductURL;

		public ProductBase(HttpClient httpClient) 
		{
			this.httpClient = httpClient;
		}

		protected async Task<Root> GetAsync(string apiString)
		{
			var response = await httpClient.GetAsync($"{ProductURL}{apiString}");

			response.EnsureSuccessStatusCode();

			return await response.Content.ReadFromJsonAsync<Root>(); ;
		}
	}
}
