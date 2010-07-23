/*
Copyright 2010 Danny Tuppeny
http://wavenotifier.dantup.com/

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

	http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/

using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using GoogleWaveNotifier.Properties;
using System.Net.NetworkInformation;

namespace GoogleWaveNotifier
{
	public partial class GoogleWaveNotifier : Form
	{
		Icon icon, iconBusy, iconGrey, iconCustom;
		ClickMode clickMode = ClickMode.OpenGoogleWave;
		int previousTotal, previousUnread;
		int isCurrentlyChecking = 0; // Can't use Interlocked on bools :(
		Brush brush = new SolidBrush(Color.DarkRed);
		Font font = new Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
		PointF badgePosition = new PointF(-1.5f, -8.5f);
		Size badgeOffset = new Size(1, 1);

		public GoogleWaveNotifier()
		{
			InitializeComponent();

			// HACK: Put localised text into labels (no nice built-in way of doing this that keeps
			// the localisations together for code-generated strings too)
			// Labels
			lblGoogleAffiliationDisclaimer.Text = Languages.InterfaceText.GoogleAffiliationDisclaimer;
			lblWebsite.Text = Languages.InterfaceText.Website;
			lblTwitter.Text = Languages.InterfaceText.Twitter;
			//lblWave.Text = Languages.InterfaceText.Wave;
			lblReportBug.Text = Languages.InterfaceText.ReportBugOrSuggestion;
			btnClose.Text = Languages.InterfaceText.Close;

			// Context menu
			googleWaveToolStripMenuItem.Text = Languages.InterfaceText.OpenGoogleWave;
			checkNowToolStripMenuItem.Text = Languages.InterfaceText.CheckNow;
			settingsToolStripMenuItem.Text = Languages.InterfaceText.Settings;
			reportABugToolStripMenuItem.Text = Languages.InterfaceText.ReportBugOrSuggestion;
			helpTranslateToolStripMenuItem.Text = Languages.InterfaceText.HelpTranslate;
			websiteToolStripMenuItem.Text = Languages.InterfaceText.Website;
			twitterToolStripMenuItem.Text = Languages.InterfaceText.Twitter;
			aboutToolStripMenuItem.Text = Languages.InterfaceText.About;
			exitToolStripMenuItem.Text = Languages.InterfaceText.Exit;
		}

		private void GoogleWaveNotifier_Load(object sender, EventArgs e)
		{
			// We want to start only shown in the tray
			this.Hide();

			// Set up display and kick off an immediate check (if we have login details)

			var ass = Assembly.GetExecutingAssembly();

			// Load version number into About dialog
			lblVersion.Text = "v" + SettingsWrapper.CurrentVersion.ToString(2);

			// Load icons
			using (Stream stream = ass.GetManifestResourceStream("GoogleWaveNotifier.Wave.ico"))
			{
				icon = new Icon(stream);
			}
			using (Stream stream = ass.GetManifestResourceStream("GoogleWaveNotifier.WaveBusy.ico"))
			{
				iconBusy = new Icon(stream);
			}
			using (Stream stream = ass.GetManifestResourceStream("GoogleWaveNotifier.WaveGrey.ico"))
			{
				iconGrey = new Icon(stream);
			}

			ResetTimerInterval();

			// If we've never set up the uesrname/password, then prompt the user
			if (string.IsNullOrEmpty(Enc.Decrypt(Settings.Default.UsernameEnc)) || string.IsNullOrEmpty(Enc.Decrypt(Settings.Default.PasswordEnc)))
				OpenSettingsDialog();
			else
				CheckWaves();

			// Check for updates from the website
			CheckForUpdates();
		}

		/// <summary>
		/// Resets the timer for when login details change, etc.
		/// </summary>
		private void ResetTimerInterval()
		{
			// Set the notify duration
			timerCheckWaves.Stop();
			timerCheckWaves.Interval = Settings.Default.CheckIntervalMinutes * 60 * 1000;
			timerCheckWaves.Start();
		}

		#region Event Handlers

		#region LinkLabels

		private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			WebBrowser.Launch("http://wavenotifier.dantup.com/");
		}

		private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			WebBrowser.Launch("http://go.dantup.com/wavenotifierbugs");
		}

		private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			WebBrowser.Launch("http://twitter.com/wavenotifier");
		}

		#endregion

		#region Context Menu

		private void googleWaveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LaunchWave();
		}

		private void checkNowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CheckWaves();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenSettingsDialog();
		}

		private void reportABugToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WebBrowser.Launch("http://go.dantup.com/wavenotifierbugs");
		}

		private void helpTranslateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WebBrowser.Launch("http://wavenotifier.dantup.com/translate");
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Show the current window (About dialog)
			this.Show();
			this.WindowState = FormWindowState.Normal;
			this.BringToFront();
		}

		private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WebBrowser.Launch("http://wavenotifier.dantup.com/");
		}

		private void twitterToolStripMenuItem_Click(object sender, EventArgs e)
		{
			WebBrowser.Launch("http://twitter.com/wavenotifier");
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// HACK: The systray icons stays visible if we just quit, so remove it first
			niSystray.Visible = false;
			Environment.Exit(0);
		}

		#endregion

		#region Systray Icon

		private void niSystray_MouseClick(object sender, MouseEventArgs e)
		{
			// Left click forces check (if enabled)
			if (Settings.Default.SingleClickForcesCheck && e.Button == MouseButtons.Left)
				CheckWaves();
		}

		private void niSystray_DoubleClick(object sender, EventArgs e)
		{
			LaunchWave();
		}

		private void niSystray_BalloonTipClicked(object sender, EventArgs e)
		{
			// Launch the right thing, based on the last balloon we showed
			switch (clickMode)
			{
				case ClickMode.OpenGoogleWave:
					LaunchWave();
					break;

				case ClickMode.OpenSettingsDialog:
					OpenSettingsDialog();
					break;

				case ClickMode.OpenWebsite:
					LaunchWebsite();
					break;
			}
		}

		#endregion

		#region Form

		private void GoogleWaveNotifier_FormClosing(object sender, FormClosingEventArgs e)
		{
			// HACK: Hijack closing, and just minimise instead. This is because this form is actually what's showing the systray icon
			// and if we close the form, the systray icon will disappear. There's probably a better way of having a NotifyIcon without
			// a form?
			if (e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.Hide();
				this.WindowState = FormWindowState.Minimized;
			}
		}

		private void lblClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		#endregion

		#region Timers

		private void timerCheckWaves_Tick(object sender, EventArgs e)
		{
			CheckWaves();
		}

		private void timerCheckForUpdates_Tick(object sender, EventArgs e)
		{
			CheckForUpdates();
		}

		#endregion

		#region Background Worker (Check Waves)

		private void bwCheck_DoWork(object sender, DoWorkEventArgs e)
		{
			niSystray.Icon = iconBusy;
			CheckWavesBackgroundThread(sender, e);
		}

		private void bwCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			CheckWavesCompleted(sender, e);
		}

		#endregion

		#region Background Worker (Check for Updates)

		private void bwCheckForUpdates_DoWork(object sender, DoWorkEventArgs e)
		{
			CheckForUpdatesBackgroundThread(sender, e);
		}

		private void bwCheckForUpdates_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			CheckForUpdatesCompleted(sender, e);
		}

		#endregion

		#endregion

		/// <summary>
		/// Launch Google Wave
		/// </summary>
		void LaunchWave()
		{
			WebBrowser.Launch(Settings.Default.WaveUrl, true);
		}

		/// <summary>
		/// Open the Settings dialog
		/// </summary>
		void OpenSettingsDialog()
		{
			using (SettingsForm frm = new SettingsForm())
			{
				frm.ShowDialog();
				ResetTimerInterval();
				CheckWaves();
			}
		}

		/// <summary>
		/// Launch Google Wave Notifier Website
		/// </summary>
		void LaunchWebsite()
		{
			WebBrowser.Launch("http://wavenotifier.dantup.com/");
		}

		private void CheckWaves()
		{
			// Don't do any work if we don't have login details
			if (string.IsNullOrEmpty(Enc.Decrypt(Settings.Default.UsernameEnc)) || string.IsNullOrEmpty(Enc.Decrypt(Settings.Default.PasswordEnc)))
				return;

			// We need to make sure we don't fire this off twice, since we're threaded
			// Set isCurrentlyChecking to true, but return the old value, so we can check if it was already running
			int alreadyChecking = Interlocked.Exchange(ref isCurrentlyChecking, 1);

			if (alreadyChecking == 1)
				return; // Quit, since another thread is already running

			// Fire off the BG thread
			bwCheck.RunWorkerAsync();
		}

		void CheckWavesBackgroundThread(object sender, DoWorkEventArgs e)
		{
			// Bail out if there's no network connection
			if (!NetworkInterface.GetIsNetworkAvailable())
				return;

			var waveClient = new GoogleWaveProxy(Enc.Decrypt(Settings.Default.UsernameEnc), Enc.Decrypt(Settings.Default.PasswordEnc));

			// Try to get the inbox
			clickMode = ClickMode.OpenGoogleWave;
			e.Result = waveClient.GetInbox();
		}

		void CheckWavesCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Error != null)
			{
				// Log the error!
				Log.Error(e.Error);

				// Show a localised error message to give a clue what the error is :)
				if (e.Error is LoginFailedException)
				{
					niSystray.ShowBalloonTip(Settings.Default.BalloonTimeoutSeconds * 1000, Languages.InterfaceText.LoginFailed, Languages.InterfaceText.LoginFailedDescription, ToolTipIcon.Error);
					clickMode = ClickMode.OpenSettingsDialog;
				}
				else if (e.Error is InvalidInboxResponseException)
				{
					niSystray.ShowBalloonTip(Settings.Default.BalloonTimeoutSeconds * 1000, Languages.InterfaceText.UnableToLoadInbox, Languages.InterfaceText.UnableToLoadInboxDescription, ToolTipIcon.Error);
					clickMode = ClickMode.OpenWebsite;
				}
				else if (e.Error is InvalidJsonException)
				{
					niSystray.ShowBalloonTip(Settings.Default.BalloonTimeoutSeconds * 1000, Languages.InterfaceText.UnableToParseInbox, Languages.InterfaceText.UnableToParseInboxDescription, ToolTipIcon.Error);
					clickMode = ClickMode.OpenWebsite;
				}
				else if (e.Error is FileNotFoundException)
				{
					niSystray.ShowBalloonTip(Settings.Default.BalloonTimeoutSeconds * 1000, Languages.InterfaceText.UnknownError, Languages.InterfaceText.UnknownErrorDescriptionDotNet, ToolTipIcon.Error);
					clickMode = ClickMode.OpenWebsite;
				}
				else
				{
					niSystray.ShowBalloonTip(Settings.Default.BalloonTimeoutSeconds * 1000, Languages.InterfaceText.UnknownError, Languages.InterfaceText.UnknownErrorDescription, ToolTipIcon.Error);
					clickMode = ClickMode.OpenWebsite;
				}
			}
			else
			{
				try
				{
					// No error, so continue processing
					WaveFolder inbox = e.Result as WaveFolder;
					if (inbox != null)
					{
						int total = 0;
						int unread = 0;
						int wavesWithUnread = 0;

						StringBuilder sbResults = new StringBuilder();

						// Loop through all waves
						foreach (var wave in inbox.Waves)
						{
							// Count how many of each message
							total += wave.TotalMessages;
							unread += wave.NewMessages;

							// If there are new messages, we'll need to build an alert
							if (wave.NewMessages > 0)
							{
								wavesWithUnread++;
								string waveTitle = string.Format("\"{0}\"", wave.OriginalMessage.Text);
								// Handle different pluralisations for different languages
								if (wave.NewMessages == 1)
									sbResults.AppendLine(string.Format(Languages.InterfaceText.NewMessagesDescription1, waveTitle));
								else if (wave.NewMessages >= 2 && wave.NewMessages <= 4)
									sbResults.AppendLine(string.Format(Languages.InterfaceText.NewMessagesDescription2to4, waveTitle, wave.NewMessages));
								else
									sbResults.AppendLine(string.Format(Languages.InterfaceText.NewMessagesDescription5plus, waveTitle, wave.NewMessages));
							}
						}

						// If there are at least some unread messages
						if (unread > 0)
						{
							// Set the "new wave" icon, we'll overlay this soon
							niSystray.Icon = icon;

							// If we have a previous drawn icon, dipose it
							if (iconCustom != null)
							{
								iconCustom.Dispose();
								iconCustom = null;
							}

							// Create a clone of the icon and add text
							using (var bmp = iconGrey.ToBitmap())
							using (var img = Graphics.FromImage(bmp))
							{
								string badge = unread > 9 ? "#" : unread.ToString();
								img.DrawString(badge, font, brush, badgePosition);
								// This will need disposing later
								iconCustom = Icon.FromHandle(bmp.GetHicon());
							}

							// Set the new icon
							niSystray.Icon = iconCustom;

							// Set the tooltip
							if (unread == 1)
								niSystray.Text = Languages.InterfaceText.NewMessagesTooltipSingleMessage;
							else if (wavesWithUnread == 1)
								niSystray.Text = string.Format(Languages.InterfaceText.NewMessagesTooltipSingleWave, unread);
							else
								niSystray.Text = string.Format(Languages.InterfaceText.NewMessagesTooltipPlural, unread, wavesWithUnread);

							// We've got some unread, but if the numbers are the same as before,
							// then it's not worth notifying. We check both because unread might
							// be the same, but it might be a different message. This isn't foolproof
							// but should work in most cases (unless a user read a message then deleted it,
							// then recieved a new one!)
							if (total != previousTotal || unread != previousUnread)
							{
								niSystray.ShowBalloonTip(Settings.Default.BalloonTimeoutSeconds * 1000, Languages.InterfaceText.NewMessages, sbResults.ToString(), ToolTipIcon.None);
								// Play the windows New Mail sound
								if (Settings.Default.PlaySounds)
								{
									try
									{
										// Wrapped in try/catch because I can't easily test this on all versions of Windows
										NativeMethods.sndPlaySoundW("MailBeep", NativeMethods.SND_ALIAS | NativeMethods.SND_NODEFAULT);
									}
									catch (Exception ex)
									{
										// Log errors, but don't ever crash!
										Log.Error(ex);
									}
								}
							}

							// Keep these counts for later
							previousTotal = total;
							previousUnread = unread;
						}
						else
						{
							// No new messages, so set the "no new wave" icon
							niSystray.Icon = iconGrey;

							niSystray.Text = Languages.InterfaceText.NoNewMessagesTooltip;
						}
					}
				}
				catch (Exception ex)
				{
					// If any error occured during this, log it an alert the user
					Log.Error(ex);
					niSystray.ShowBalloonTip(Settings.Default.BalloonTimeoutSeconds * 1000, Languages.InterfaceText.UnknownError, Languages.InterfaceText.UnknownErrorDescription, ToolTipIcon.Error);
				}
			}

			isCurrentlyChecking = 0;
		}

		void CheckForUpdates()
		{
			// Check for updates
			bwCheckForUpdates.RunWorkerAsync();
		}

		void CheckForUpdatesBackgroundThread(object sender, DoWorkEventArgs e)
		{
			// HACK: Grab the version number from the website. This should have its own service
			// but it'll work for now.
			using (var client = new WaveWebClient())
			{
				var results = client.DownloadString("http://wavenotifier.dantup.com/?vercheck");

				var match = Regex.Match(results, @"The current version is v(\d+\.\d+)");
				if (match.Success)
				{
					e.Result = new Version(match.Groups[1].Value);
				}
			}
		}

		void CheckForUpdatesCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (e.Error == null)
			{
				// Get the version numbers
				Version latestVersion = e.Result as Version;
				Version myVersion = Assembly.GetExecutingAssembly().GetName().Version;

				// If there's a new version available, alert the uer
				if (latestVersion > myVersion)
				{
					NotifyUpdate(latestVersion, myVersion);
				}
			}
		}

		void NotifyUpdate(Version latestVersion, Version myVersion)
		{
			// Show a balloon telling the user there's a new version
			string message = string.Format(Languages.InterfaceText.NewVersionAvailableDescription, latestVersion.ToString(2), myVersion.ToString(2));
			niSystray.ShowBalloonTip(Settings.Default.BalloonTimeoutSeconds * 1000, Languages.InterfaceText.NewVersionAvailable, message, ToolTipIcon.Info);
			clickMode = ClickMode.OpenWebsite;
		}
	}

	/// <summary>
	/// ClickMode. Used to keep track of the last balloon we showed, so we can handle the BalloonClicked event
	/// </summary>
	enum ClickMode
	{
		OpenGoogleWave,
		OpenSettingsDialog,
		OpenWebsite
	}
}
