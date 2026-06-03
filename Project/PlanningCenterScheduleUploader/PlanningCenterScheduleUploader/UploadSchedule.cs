using PlanningCenterAPI.Helper;
using PlanningCenterScheduleUploaderLib;
using PlanningCenterScheduleUploaderLib.Process.Implementation;

namespace PlanningCenterScheduleUploader
{
    public partial class UploadSchedule : Form
    {
        private string scheduleFilename = string.Empty;
        private bool isUploading;
        private string _updateUrl = string.Empty;

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

        private async void btnUpload_Click(object sender, EventArgs e)
        {
            if (isUploading)
                return;

            isUploading = true;
            btnUpload.Enabled = false;

            await RunUpload();
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
            try
            {
                var text = string.Join(Environment.NewLine, lstBxError.Items.Cast<string>());
                Clipboard.SetText(text);
            }
            catch
            {
                MessageBox.Show(
                    "Could not copy the log to the clipboard. Please try again, or select the log entries manually.",
                    "Copy Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(_updateUrl) { UseShellExecute = true });
            }
            catch
            {
                MessageBox.Show(
                    $"Could not open the browser. Please visit the releases page manually:\n{_updateUrl}",
                    "Update",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private async Task CheckForUpdatesAsync()
        {
            var result = await UpdateChecker.CheckForUpdateAsync();
            if (!result.HasUpdate)
                return;

            _updateUrl = result.ReleaseUrl;
            btnUpdate.Text = $"Update Available: {result.LatestTag} — Click to Download";
            btnUpdate.Visible = true;

            btnFile.Enabled = false;
            btnSettings.Enabled = false;
            btnUpload.Enabled = false;
            btnCopyLogs.Enabled = false;
        }

        private void WriteLine(string s)
        {
            lstBxError.Items.Add(s);
        }
    }
}
