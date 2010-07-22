namespace GoogleWaveNotifier
{
	partial class GoogleWaveNotifier
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoogleWaveNotifier));
			this.niSystray = new System.Windows.Forms.NotifyIcon(this.components);
			this.cmSystray = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.googleWaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.checkNowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.reportABugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpTranslateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.twitterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.websiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.lblVersion = new System.Windows.Forms.Label();
			this.timerCheckWaves = new System.Windows.Forms.Timer(this.components);
			this.lblWebsite = new System.Windows.Forms.LinkLabel();
			this.lblReportBug = new System.Windows.Forms.LinkLabel();
			this.lblGoogleAffiliationDisclaimer = new System.Windows.Forms.Label();
			this.bwCheck = new System.ComponentModel.BackgroundWorker();
			this.bwCheckForUpdates = new System.ComponentModel.BackgroundWorker();
			this.lblTwitter = new System.Windows.Forms.LinkLabel();
			this.label7 = new System.Windows.Forms.Label();
			this.lblClose = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.timerCheckForUpdates = new System.Windows.Forms.Timer(this.components);
			this.cmSystray.SuspendLayout();
			this.SuspendLayout();
			// 
			// niSystray
			// 
			this.niSystray.ContextMenuStrip = this.cmSystray;
			this.niSystray.Icon = ((System.Drawing.Icon)(resources.GetObject("niSystray.Icon")));
			this.niSystray.Text = "Google Wave Notifier by Danny Tuppeny";
			this.niSystray.Visible = true;
			this.niSystray.BalloonTipClicked += new System.EventHandler(this.niSystray_BalloonTipClicked);
			this.niSystray.DoubleClick += new System.EventHandler(this.niSystray_DoubleClick);
			this.niSystray.MouseClick += new System.Windows.Forms.MouseEventHandler(this.niSystray_MouseClick);
			// 
			// cmSystray
			// 
			this.cmSystray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.googleWaveToolStripMenuItem,
            this.checkNowToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.toolStripSeparator2,
            this.reportABugToolStripMenuItem,
            this.helpTranslateToolStripMenuItem,
            this.toolStripSeparator3,
            this.aboutToolStripMenuItem,
            this.twitterToolStripMenuItem,
            this.websiteToolStripMenuItem,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
			this.cmSystray.Name = "cmSystray";
			this.cmSystray.Size = new System.Drawing.Size(231, 220);
			// 
			// googleWaveToolStripMenuItem
			// 
			this.googleWaveToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.googleWaveToolStripMenuItem.Name = "googleWaveToolStripMenuItem";
			this.googleWaveToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.googleWaveToolStripMenuItem.Text = "##OpenGoogleWave##";
			this.googleWaveToolStripMenuItem.Click += new System.EventHandler(this.googleWaveToolStripMenuItem_Click);
			// 
			// checkNowToolStripMenuItem
			// 
			this.checkNowToolStripMenuItem.Name = "checkNowToolStripMenuItem";
			this.checkNowToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.checkNowToolStripMenuItem.Text = "##CheckNow##";
			this.checkNowToolStripMenuItem.Click += new System.EventHandler(this.checkNowToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.settingsToolStripMenuItem.Text = "##Settings##";
			this.settingsToolStripMenuItem.Click += new System.EventHandler(this.settingsToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(227, 6);
			// 
			// reportABugToolStripMenuItem
			// 
			this.reportABugToolStripMenuItem.Name = "reportABugToolStripMenuItem";
			this.reportABugToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.reportABugToolStripMenuItem.Text = "##ReportBugOrSuggestion##";
			this.reportABugToolStripMenuItem.Click += new System.EventHandler(this.reportABugToolStripMenuItem_Click);
			// 
			// helpTranslateToolStripMenuItem
			// 
			this.helpTranslateToolStripMenuItem.Name = "helpTranslateToolStripMenuItem";
			this.helpTranslateToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.helpTranslateToolStripMenuItem.Text = "##HelpTranslate##";
			this.helpTranslateToolStripMenuItem.Click += new System.EventHandler(this.helpTranslateToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(227, 6);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.aboutToolStripMenuItem.Text = "##About##";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// twitterToolStripMenuItem
			// 
			this.twitterToolStripMenuItem.Name = "twitterToolStripMenuItem";
			this.twitterToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.twitterToolStripMenuItem.Text = "##Twitter##";
			this.twitterToolStripMenuItem.Click += new System.EventHandler(this.twitterToolStripMenuItem_Click);
			// 
			// websiteToolStripMenuItem
			// 
			this.websiteToolStripMenuItem.Name = "websiteToolStripMenuItem";
			this.websiteToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.websiteToolStripMenuItem.Text = "##Website##";
			this.websiteToolStripMenuItem.Click += new System.EventHandler(this.websiteToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(227, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
			this.exitToolStripMenuItem.Text = "##Exit##";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// lblVersion
			// 
			this.lblVersion.AutoSize = true;
			this.lblVersion.BackColor = System.Drawing.Color.Transparent;
			this.lblVersion.ForeColor = System.Drawing.Color.White;
			this.lblVersion.Location = new System.Drawing.Point(12, 148);
			this.lblVersion.Name = "lblVersion";
			this.lblVersion.Size = new System.Drawing.Size(28, 13);
			this.lblVersion.TabIndex = 10;
			this.lblVersion.Text = "v0.0";
			// 
			// timerCheckWaves
			// 
			this.timerCheckWaves.Enabled = true;
			this.timerCheckWaves.Interval = 900000;
			this.timerCheckWaves.Tick += new System.EventHandler(this.timerCheckWaves_Tick);
			// 
			// lblWebsite
			// 
			this.lblWebsite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblWebsite.BackColor = System.Drawing.Color.Transparent;
			this.lblWebsite.Location = new System.Drawing.Point(17, 86);
			this.lblWebsite.Name = "lblWebsite";
			this.lblWebsite.Size = new System.Drawing.Size(123, 13);
			this.lblWebsite.TabIndex = 5;
			this.lblWebsite.TabStop = true;
			this.lblWebsite.Text = "##Website##";
			this.lblWebsite.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
			// 
			// lblReportBug
			// 
			this.lblReportBug.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblReportBug.BackColor = System.Drawing.Color.Transparent;
			this.lblReportBug.Location = new System.Drawing.Point(97, 86);
			this.lblReportBug.Name = "lblReportBug";
			this.lblReportBug.Size = new System.Drawing.Size(206, 13);
			this.lblReportBug.TabIndex = 11;
			this.lblReportBug.TabStop = true;
			this.lblReportBug.Text = "##ReportBugOrSuggestion##";
			this.lblReportBug.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.lblReportBug.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
			// 
			// lblGoogleAffiliationDisclaimer
			// 
			this.lblGoogleAffiliationDisclaimer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lblGoogleAffiliationDisclaimer.BackColor = System.Drawing.Color.Transparent;
			this.lblGoogleAffiliationDisclaimer.ForeColor = System.Drawing.Color.Black;
			this.lblGoogleAffiliationDisclaimer.Location = new System.Drawing.Point(17, 39);
			this.lblGoogleAffiliationDisclaimer.Name = "lblGoogleAffiliationDisclaimer";
			this.lblGoogleAffiliationDisclaimer.Size = new System.Drawing.Size(286, 47);
			this.lblGoogleAffiliationDisclaimer.TabIndex = 1;
			this.lblGoogleAffiliationDisclaimer.Text = "##GoogleAffiliationDisclaimer##";
			// 
			// bwCheck
			// 
			this.bwCheck.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwCheck_DoWork);
			this.bwCheck.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwCheck_RunWorkerCompleted);
			// 
			// bwCheckForUpdates
			// 
			this.bwCheckForUpdates.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwCheckForUpdates_DoWork);
			this.bwCheckForUpdates.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwCheckForUpdates_RunWorkerCompleted);
			// 
			// lblTwitter
			// 
			this.lblTwitter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.lblTwitter.AutoSize = true;
			this.lblTwitter.BackColor = System.Drawing.Color.Transparent;
			this.lblTwitter.Location = new System.Drawing.Point(17, 111);
			this.lblTwitter.Name = "lblTwitter";
			this.lblTwitter.Size = new System.Drawing.Size(67, 13);
			this.lblTwitter.TabIndex = 7;
			this.lblTwitter.TabStop = true;
			this.lblTwitter.Text = "##Twitter##";
			this.lblTwitter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.AutoSize = true;
			this.label7.BackColor = System.Drawing.Color.Transparent;
			this.label7.ForeColor = System.Drawing.Color.Black;
			this.label7.Location = new System.Drawing.Point(134, 111);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(169, 13);
			this.label7.TabIndex = 9;
			this.label7.Text = "danny.tuppeny@googlewave.com";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// lblClose
			// 
			this.lblClose.BackColor = System.Drawing.Color.Transparent;
			this.lblClose.Cursor = System.Windows.Forms.Cursors.Hand;
			this.lblClose.Location = new System.Drawing.Point(269, 2);
			this.lblClose.Name = "lblClose";
			this.lblClose.Size = new System.Drawing.Size(52, 45);
			this.lblClose.TabIndex = 12;
			this.lblClose.Click += new System.EventHandler(this.lblClose_Click);
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point(244, 180);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 13;
			this.btnClose.Text = "##Close##";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
			// 
			// timerCheckForUpdates
			// 
			this.timerCheckForUpdates.Enabled = true;
			this.timerCheckForUpdates.Interval = 43200000;
			this.timerCheckForUpdates.Tick += new System.EventHandler(this.timerCheckForUpdates_Tick);
			// 
			// GoogleWaveNotifier
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(323, 175);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.lblGoogleAffiliationDisclaimer);
			this.Controls.Add(this.lblVersion);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.lblReportBug);
			this.Controls.Add(this.lblTwitter);
			this.Controls.Add(this.lblWebsite);
			this.Controls.Add(this.lblClose);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GoogleWaveNotifier";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Google Wave Notifier";
			this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			this.Load += new System.EventHandler(this.GoogleWaveNotifier_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GoogleWaveNotifier_FormClosing);
			this.cmSystray.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.NotifyIcon niSystray;
		private System.Windows.Forms.ContextMenuStrip cmSystray;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem googleWaveToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.Timer timerCheckWaves;
		private System.Windows.Forms.ToolStripMenuItem checkNowToolStripMenuItem;
		private System.Windows.Forms.LinkLabel lblWebsite;
		private System.Windows.Forms.LinkLabel lblReportBug;
		private System.Windows.Forms.Label lblGoogleAffiliationDisclaimer;
		private System.Windows.Forms.ToolStripMenuItem reportABugToolStripMenuItem;
		private System.ComponentModel.BackgroundWorker bwCheck;
		private System.ComponentModel.BackgroundWorker bwCheckForUpdates;
		private System.Windows.Forms.LinkLabel lblTwitter;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.ToolStripMenuItem helpTranslateToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem websiteToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem twitterToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.Label lblClose;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Timer timerCheckForUpdates;
	}
}

