namespace PlanningCenterScheduleUploader
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblDescription = new Label();
            lblAppId = new Label();
            txtAppId = new TextBox();
            lblSecret = new Label();
            txtSecret = new TextBox();
            lnkHelp = new LinkLabel();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();

            // lblDescription
            lblDescription.Location = new Point(12, 12);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(360, 40);
            lblDescription.Text = "Enter your Planning Center Personal Access Token credentials. These are stored locally on your computer.";

            // lblAppId
            lblAppId.AutoSize = true;
            lblAppId.Location = new Point(12, 60);
            lblAppId.Name = "lblAppId";
            lblAppId.Text = "Application ID:";

            // txtAppId
            txtAppId.Location = new Point(12, 78);
            txtAppId.Name = "txtAppId";
            txtAppId.Size = new Size(360, 23);
            txtAppId.TabIndex = 0;

            // lblSecret
            lblSecret.AutoSize = true;
            lblSecret.Location = new Point(12, 113);
            lblSecret.Name = "lblSecret";
            lblSecret.Text = "Secret:";

            // txtSecret
            txtSecret.Location = new Point(12, 131);
            txtSecret.Name = "txtSecret";
            txtSecret.Size = new Size(360, 23);
            txtSecret.TabIndex = 1;
            txtSecret.UseSystemPasswordChar = true;

            // lnkHelp
            lnkHelp.AutoSize = true;
            lnkHelp.Location = new Point(12, 162);
            lnkHelp.Name = "lnkHelp";
            lnkHelp.TabIndex = 2;
            lnkHelp.TabStop = true;
            lnkHelp.Text = "How to get your credentials at Planning Center ↗";
            lnkHelp.LinkClicked += lnkHelp_LinkClicked;

            // btnSave
            btnSave.Location = new Point(216, 190);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 23);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;

            // btnCancel
            btnCancel.Location = new Point(297, 190);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 4;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;

            // SettingsForm
            AcceptButton = btnSave;
            CancelButton = btnCancel;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(396, 225);
            Controls.Add(lblDescription);
            Controls.Add(lblAppId);
            Controls.Add(txtAppId);
            Controls.Add(lblSecret);
            Controls.Add(txtSecret);
            Controls.Add(lnkHelp);
            Controls.Add(btnSave);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SettingsForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Planning Center Credentials";
            ResumeLayout(false);
            PerformLayout();
        }

        private Label lblDescription;
        private Label lblAppId;
        private TextBox txtAppId;
        private Label lblSecret;
        private TextBox txtSecret;
        private LinkLabel lnkHelp;
        private Button btnSave;
        private Button btnCancel;
    }
}
