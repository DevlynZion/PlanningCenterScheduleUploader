using PlanningCenterAPI.Helper;
using PlanningCenterScheduleUploaderLib;
using PlanningCenterScheduleUploaderLib.Process.Implementation;

namespace PlanningCenterScheduleUploader
{
    public partial class UploadSchedule : Form
    {
        private string scheduleFilename = string.Empty;
        private bool isUploading;

        public UploadSchedule()
        {
            InitializeComponent();
        }

        private async void UploadSchedule_Load(object sender, EventArgs e)
        {
            if (!AuthenticationHelper.CredentialsExist())
                OpenSettings(firstLaunch: true);

            await CheckForUpdatesAsync();
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

        private void btnSettings_Click(object sender, EventArgs e)
        {
            OpenSettings(firstLaunch: false);
        }

        private void OpenSettings(bool firstLaunch)
        {
            using var form = new SettingsForm(firstLaunch);
            form.ShowDialog(this);
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            if (isUploading)
                return;

            isUploading = true;
            btnUpload.Enabled = false;

            _ = RunUpload();
        }

        private async Task RunUpload()
        {
            lstBxError.Items.Clear();
            btnCopyLogs.Enabled = false;
            WriteLine($"Uploading: {scheduleFilename}");

            var excelProcessor = new ExcelProcessor(scheduleFilename);
            PlanningCenterManager planningCenterManager = new PlanningCenterManager(excelProcessor);
            try
            {
                await planningCenterManager.Start();
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("Credentials not configured"))
            {
                WriteLine("Credentials not configured — click Settings to enter your Planning Center API credentials.");
            }
            catch (Exception ex)
            {
                WriteLine(ex.ToString());
                WriteLine(ex.StackTrace?.ToString() ?? string.Empty);
            }
            finally
            {
                if (planningCenterManager.AnyErrors)
                {
                    WriteLine("Error");
                    WriteLine("=====");
                    foreach (var error in planningCenterManager.Errors)
                        WriteLine($"{error.ErrorLevel} - [{error.CellCoordinate.TabName}, {error.CellCoordinate.RowNumber + 1}, {error.CellCoordinate.ColumnIndex + 1}] {error.Message}");
                }

                btnUpload.Enabled = true;
                isUploading = false;
                btnCopyLogs.Enabled = lstBxError.Items.Count > 0;
            }
        }

        private void btnCopyLogs_Click(object sender, EventArgs e)
        {
            var text = string.Join(Environment.NewLine, lstBxError.Items.Cast<string>());
            Clipboard.SetText(text);
        }

        private void lnkUpdate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Link?.LinkData is string url)
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(url) { UseShellExecute = true });
        }

        private async Task CheckForUpdatesAsync()
        {
            var result = await UpdateChecker.CheckForUpdateAsync();
            if (!result.HasUpdate)
                return;

            lnkUpdate.Text = $"Update available: {result.LatestTag} — click to download";
            lnkUpdate.Links.Clear();
            lnkUpdate.Links.Add(0, lnkUpdate.Text.Length, result.ReleaseUrl);
            lnkUpdate.Visible = true;
        }

        private void WriteLine(string s)
        {
            lstBxError.Items.Add(s);
        }
    }
}
