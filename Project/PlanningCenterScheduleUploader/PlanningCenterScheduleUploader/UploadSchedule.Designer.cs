namespace PlanningCenterScheduleUploader
{
	partial class UploadSchedule
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnFile = new Button();
            btnUpload = new Button();
            lstBxError = new ListBox();
            SuspendLayout();
            // 
            // btnFile
            // 
            btnFile.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnFile.Location = new Point(12, 12);
            btnFile.Name = "btnFile";
            btnFile.Size = new Size(481, 23);
            btnFile.TabIndex = 0;
            btnFile.Text = "Set File";
            btnFile.UseVisualStyleBackColor = true;
            btnFile.Click += btnFile_Click;
            // 
            // btnUpload
            // 
            btnUpload.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnUpload.Enabled = false;
            btnUpload.Location = new Point(12, 41);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(481, 23);
            btnUpload.TabIndex = 1;
            btnUpload.Text = "Upload";
            btnUpload.UseVisualStyleBackColor = true;
            btnUpload.Click += btnUpload_Click;
            // 
            // lstBxError
            // 
            lstBxError.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lstBxError.FormattingEnabled = true;
            lstBxError.ItemHeight = 15;
            lstBxError.Location = new Point(12, 70);
            lstBxError.Name = "lstBxError";
            lstBxError.Size = new Size(481, 184);
            lstBxError.TabIndex = 2;
            // 
            // UploadSchedule
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 261);
            Controls.Add(lstBxError);
            Controls.Add(btnUpload);
            Controls.Add(btnFile);
            MinimumSize = new Size(520, 300);
            Name = "UploadSchedule";
            Text = "Upload Schedule";
            ResumeLayout(false);
        }

        #endregion

        private Button btnFile;
        private Button btnUpload;
        private ListBox lstBxError;
    }
}
