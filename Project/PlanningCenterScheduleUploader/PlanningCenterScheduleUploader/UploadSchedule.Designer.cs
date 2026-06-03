namespace PlanningCenterScheduleUploader
{
    partial class UploadSchedule
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
            btnFile = new Button();
            btnSettings = new Button();
            btnUpload = new Button();
            lstBxError = new ListBox();
            btnCopyLogs = new Button();
            lnkUpdate = new LinkLabel();
            SuspendLayout();

            // btnFile
            btnFile.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnFile.Location = new Point(12, 12);
            btnFile.Name = "btnFile";
            btnFile.Size = new Size(374, 23);
            btnFile.TabIndex = 0;
            btnFile.Text = "Set File";
            btnFile.UseVisualStyleBackColor = true;
            btnFile.Click += btnFile_Click;

            // btnSettings
            btnSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSettings.Location = new Point(392, 12);
            btnSettings.Name = "btnSettings";
            btnSettings.Size = new Size(100, 23);
            btnSettings.TabIndex = 1;
            btnSettings.Text = "Settings";
            btnSettings.UseVisualStyleBackColor = true;
            btnSettings.Click += btnSettings_Click;

            // btnUpload
            btnUpload.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnUpload.Enabled = false;
            btnUpload.Location = new Point(12, 41);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(481, 23);
            btnUpload.TabIndex = 2;
            btnUpload.Text = "Upload";
            btnUpload.UseVisualStyleBackColor = true;
            btnUpload.Click += btnUpload_Click;

            // lstBxError
            lstBxError.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstBxError.FormattingEnabled = true;
            lstBxError.ItemHeight = 15;
            lstBxError.Location = new Point(12, 70);
            lstBxError.Name = "lstBxError";
            lstBxError.Size = new Size(481, 179);
            lstBxError.TabIndex = 3;

            // btnCopyLogs
            btnCopyLogs.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnCopyLogs.Enabled = false;
            btnCopyLogs.Location = new Point(12, 257);
            btnCopyLogs.Name = "btnCopyLogs";
            btnCopyLogs.Size = new Size(120, 23);
            btnCopyLogs.TabIndex = 4;
            btnCopyLogs.Text = "Copy Logs";
            btnCopyLogs.UseVisualStyleBackColor = true;
            btnCopyLogs.Click += btnCopyLogs_Click;

            // lnkUpdate
            lnkUpdate.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lnkUpdate.AutoSize = false;
            lnkUpdate.Location = new Point(12, 284);
            lnkUpdate.Name = "lnkUpdate";
            lnkUpdate.Size = new Size(481, 15);
            lnkUpdate.TabIndex = 5;
            lnkUpdate.TabStop = true;
            lnkUpdate.Text = string.Empty;
            lnkUpdate.Visible = false;
            lnkUpdate.LinkClicked += lnkUpdate_LinkClicked;

            // UploadSchedule
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 307);
            Controls.Add(lnkUpdate);
            Controls.Add(btnCopyLogs);
            Controls.Add(lstBxError);
            Controls.Add(btnUpload);
            Controls.Add(btnSettings);
            Controls.Add(btnFile);
            MinimumSize = new Size(520, 346);
            Name = "UploadSchedule";
            Text = "Upload Schedule";
            Load += UploadSchedule_Load;
            ResumeLayout(false);
        }

        private Button btnFile;
        private Button btnSettings;
        private Button btnUpload;
        private ListBox lstBxError;
        private Button btnCopyLogs;
        private LinkLabel lnkUpdate;
    }
}
