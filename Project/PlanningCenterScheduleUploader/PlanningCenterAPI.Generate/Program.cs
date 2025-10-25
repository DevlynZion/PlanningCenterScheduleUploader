using Newtonsoft.Json.Schema;
using NJsonSchema.CodeGeneration.CSharp;
using PlanningCenterAPI.Generate.Core;

namespace PlanningCenterAPI.Generate
{
	internal class Program
	{
		static void Main(string[] args)
		{
			using (Client client = new Client())
			{
				var JSON = client.Get(@"https://api.planningcenteronline.com/services/v2/teams/5948513?include=team_positions").GetAwaiter();

				var schemaFromFile = JsonSchema.Parse(JSON.GetResult());

				var classGenerator = new CSharpGenerator(schemaFromFile, new CSharpGeneratorSettings
				{
					ClassStyle = CSharpClassStyle.Poco,
				});

				var codeFile = classGenerator.GenerateFile();
				File.WriteAllText("Test.cs", codeFile);
			}
			
			Console.WriteLine("Done.");
		}
	}
}
