using PlanningCenterAPI.Helper;

namespace PlanningCenterScheduleUploader
{
    public partial class SettingsForm : Form
    {
        public SettingsForm(bool firstLaunch = false)
        {
            InitializeComponent();

            if (firstLaunch)
                Text = "Welcome — Set Up Your Credentials";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var appId = txtAppId.Text.Trim();
            var secret = txtSecret.Text.Trim();

            if (string.IsNullOrEmpty(appId) || string.IsNullOrEmpty(secret))
            {
                MessageBox.Show(
                    "Both Application ID and Secret are required.",
                    "Missing Credentials",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            AuthenticationHelper.SaveCredentials(appId, secret);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void lnkHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(
                "https://api.planningcenteronline.com/oauth/applications")
            {
                UseShellExecute = true
            });
        }
    }
}
