using PlanningCenterScheduleUploaderLib;
using PlanningCenterScheduleUploaderLib.Process.Implementation;
using System.Security.Cryptography;

namespace PlanningCenterScheduleUploader
{
	public partial class UploadSchedule : Form
	{
        private string scheduleFilename;

		public UploadSchedule()
		{
			InitializeComponent();
		}

		private void btnFile_Click(object sender, EventArgs e)
		{
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select Schedule File";

            if (ofd.ShowDialog() != DialogResult.OK)
                return;

            scheduleFilename = ofd.FileName;

            btnUpload.Enabled = true;
		}

        private void btnUpload_Click(object sender, EventArgs e)
        {
            Process();
        }

        private async Task Process()
        {
            lstBxError.Items.Clear();
            WriteLine($"Uploading: {scheduleFilename}");
            var excelProcessor = new ExcelProcessor(scheduleFilename);
            PlanningCenterManager planningCenterManager = new PlanningCenterManager(excelProcessor);
            try
            {
                await planningCenterManager.Start();
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
                WriteLine(ex.StackTrace?.ToString());
            }
            finally
            {
                if (planningCenterManager.AnyErrors)
                {
                    WriteLine("Error");
                    WriteLine("=====");
                    foreach (var error in planningCenterManager.Errors)
                        WriteLine($"{error.ErrorLevel.ToString()} - [{error.CellCoordinate.TabName}, {error.CellCoordinate.RowNumber + 1}, {error.CellCoordinate.ColumnIndex + 1}] {error.Message}");
                }
            }
        }

        private void WriteLine(string s)
        {
            lstBxError.Items.Add(s);
        }
    }
}
