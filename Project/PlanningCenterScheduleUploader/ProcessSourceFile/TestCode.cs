using ExcelDataReader;
using System.Data;

namespace ProcessSourceFile
{
	public class TestCode
	{
		private const string filePath = @"./Schedule.xlsx";
		public async Task Run()
		{
			// Using: https://github.com/ExcelDataReader/ExcelDataReader

			System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

			using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
			using (var reader = ExcelReaderFactory.CreateReader(stream))
			{
				var result = reader.AsDataSet();
				var Setup = result.Tables["Setup"];
				var Schedule = result.Tables["Schedule"];

				foreach(DataRow setupRow in Setup.Rows)
				{
					var configName = setupRow[0] as string;
					var configVale = setupRow[1] as string;

					Console.WriteLine($"{configName}=:{configVale}");
				}
			}
		}
	}
}
