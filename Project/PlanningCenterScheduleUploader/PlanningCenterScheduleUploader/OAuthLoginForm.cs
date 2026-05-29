using PlanningCenterAPI.OAuth;

namespace PlanningCenterScheduleUploader
{
    /// <summary>
    /// OAuth login form — prompts the user to log in via Planning Center.
    /// Opens the browser and waits for the callback. Returns tokens on success.
    /// </summary>
    public partial class OAuthLoginForm : Form
    {
        private readonly OAuthConfig _config;
        private CancellationTokenSource? _cts;
        private OAuthLoginResult? _result;

        public OAuthLoginResult? LoginResult => _result;

        private System.ComponentModel.IContainer? components;

        public OAuthLoginForm(OAuthConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            components = new System.ComponentModel.Container();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = "Login to Planning Center";
            this.Size = new System.Drawing.Size(480, 220);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            var lblTitle = new Label
            {
                Text = "Login Required",
                Font = new System.Drawing.Font("Segoe UI", 16, System.Drawing.FontStyle.Bold),
                AutoSize = true,
                Location = new System.Drawing.Point(30, 20)
            };

            var lblDescription = new Label
            {
                Text = "Click the button below to open your browser and sign in with your Planning Center account.",
                Location = new System.Drawing.Point(30, 60),
                Size = new System.Drawing.Size(410, 40),
                Font = new System.Drawing.Font("Segoe UI", 9)
            };

            var lblStatus = new Label
            {
                Name = "lblStatus",
                Text = "",
                Location = new System.Drawing.Point(30, 105),
                Size = new System.Drawing.Size(410, 20),
                ForeColor = System.Drawing.Color.Black,
                Font = new System.Drawing.Font("Segoe UI", 9)
            };

            var btnLogin = new Button
            {
                Name = "btnLogin",
                Text = "Log in with Planning Center",
                Location = new System.Drawing.Point(30, 130),
                Size = new System.Drawing.Size(200, 35),
                Font = new System.Drawing.Font("Segoe UI", 9)
            };
            btnLogin.Click += BtnLogin_Click;

            var btnCancel = new Button
            {
                Name = "btnCancel",
                Text = "Cancel",
                Location = new System.Drawing.Point(250, 130),
                Size = new System.Drawing.Size(100, 35),
                Font = new System.Drawing.Font("Segoe UI", 9),
                DialogResult = DialogResult.Cancel
            };
            btnCancel.Click += BtnCancel_Click;

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblDescription);
            this.Controls.Add(lblStatus);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnCancel);

            this.AcceptButton = btnLogin;
            this.CancelButton = btnCancel;
        }

        private async void BtnLogin_Click(object? sender, EventArgs e)
        {
            var btnLogin = this.Controls.Find("btnLogin", false).FirstOrDefault() as Button;
            var btnCancel = this.Controls.Find("btnCancel", false).FirstOrDefault() as Button;
            var lblStatus = this.Controls.Find("lblStatus", false).FirstOrDefault() as Label;

            if (btnLogin is not null) btnLogin.Enabled = false;
            if (btnCancel is not null) btnCancel.Enabled = false;
            if (lblStatus is not null) lblStatus.Text = "Opening browser...";

            _cts = new CancellationTokenSource();

            try
            {
                var loginService = new OAuthLoginService(_config);
                _result = await loginService.LoginAsync(_cts.Token);

                if (_result.Success)
                {
                    if (lblStatus is not null)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Green;
                        lblStatus.Text = "Login successful!";
                    }

                    // Brief pause so the user sees the success message
                    await Task.Delay(1500);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    if (lblStatus is not null)
                    {
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        lblStatus.Text = $"Login failed: {_result.Error}";
                    }
                    if (btnLogin is not null) btnLogin.Enabled = true;
                    if (btnCancel is not null) btnCancel.Enabled = true;
                }
            }
            catch (OperationCanceledException)
            {
                if (lblStatus is not null)
                {
                    lblStatus.ForeColor = System.Drawing.Color.Orange;
                    lblStatus.Text = "Login cancelled.";
                }
                if (btnLogin is not null) btnLogin.Enabled = true;
                if (btnCancel is not null) btnCancel.Enabled = true;
            }
            catch (Exception ex)
            {
                if (lblStatus is not null)
                {
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                    lblStatus.Text = $"Error: {ex.Message}";
                }
                if (btnLogin is not null) btnLogin.Enabled = true;
                if (btnCancel is not null) btnCancel.Enabled = true;
            }
            finally
            {
                _cts?.Dispose();
                _cts = null;
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            _cts?.Cancel();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cts?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
