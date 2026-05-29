using PlanningCenterAPI;
using PlanningCenterAPI.OAuth;

namespace PlanningCenterScheduleUploader
{
    public partial class Form1 : Form
    {
        private OAuthAuthenticator? _authenticator;
        private PlanningCenter? _planningCenter;

        public Form1()
        {
            InitializeComponent();
            SetupLoginUI();
        }

        private void SetupLoginUI()
        {
            this.Text = "Planning Center Schedule Uploader";

            var btnLogin = new Button
            {
                Name = "btnLogin",
                Text = "Login with Planning Center",
                Location = new System.Drawing.Point(30, 30),
                Size = new System.Drawing.Size(220, 40),
                Font = new System.Drawing.Font("Segoe UI", 10)
            };
            btnLogin.Click += BtnLogin_Click;

            var lblStatus = new Label
            {
                Name = "lblStatus",
                Text = "Not logged in",
                Location = new System.Drawing.Point(270, 40),
                AutoSize = true,
                Font = new System.Drawing.Font("Segoe UI", 10)
            };

            this.Controls.Add(btnLogin);
            this.Controls.Add(lblStatus);
        }

        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            var lblStatus = this.Controls.Find("lblStatus", false).FirstOrDefault() as Label;
            var btnLogin = this.Controls.Find("btnLogin", false).FirstOrDefault() as Button;

            // TODO: Load these from a config file or settings
            var config = new OAuthConfig
            {
                ClientId = "YOUR_CLIENT_ID",
                ClientSecret = "YOUR_CLIENT_SECRET",
                RedirectUri = "http://localhost:7168/callback",
                Scopes = "people services"
            };

            if (btnLogin is not null) btnLogin.Enabled = false;
            if (lblStatus is not null) lblStatus.Text = "Waiting for login...";

            using (var loginForm = new OAuthLoginForm(config))
            {
                if (loginForm.ShowDialog(this) == DialogResult.OK && loginForm.LoginResult is not null)
                {
                    _authenticator = new OAuthAuthenticator(config);
                    await _authenticator.InitializeAsync();

                    _planningCenter = new PlanningCenter(
                        _authenticator.GetTokenProvider());

                    if (lblStatus is not null)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        lblStatus.Text = "Logged in successfully!";
                    }
                }
                else
                {
                    if (lblStatus is not null)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = "Login failed or cancelled.";
                    }
                }
            }

            if (btnLogin is not null) btnLogin.Enabled = true;
        }
    }
}
