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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GoogleWaveNotifier.Properties;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace GoogleWaveNotifier
{
	/// <summary>
	/// Used for editing Settings via Property Grid
	/// </summary>
	class SettingsWrapper
	{
		/// <summary>
		/// The current version of the app
		/// </summary>
		static public Version CurrentVersion { get; private set; } // Set in static constructor

		/// <summary>
		/// Private constructor, so only we can create instances
		/// </summary>
		private SettingsWrapper()
		{
		}

		/// <summary>
		/// Gets a copy of the settings for editing
		/// </summary>
		/// <returns></returns>
		public static SettingsWrapper Get()
		{
			var settings = new SettingsWrapper();

			// Copy settings
			settings.Username = Enc.Decrypt(Settings.Default.UsernameEnc);
			settings.Password = Enc.Decrypt(Settings.Default.PasswordEnc);
			settings.CheckIntervalMinutes = Settings.Default.CheckIntervalMinutes;
			settings.WaveUrl = Settings.Default.WaveUrl;
			settings.PlaySounds = Settings.Default.PlaySounds;
			settings.SingleClickForcesCheck = Settings.Default.SingleClickForcesCheck;
			settings.BalloonTimeout = Settings.Default.BalloonTimeoutSeconds;
			settings.PreferredCulture = Settings.Default.PreferredCulture;
			settings.PreferredBrowser = Settings.Default.PreferredBrowser;
			settings.RunAtStartup = Settings.Default.RunAtStartup;
			settings.ProxyAddress = Settings.Default.ProxyAddress;
			settings.ProxyUsername = Enc.Decrypt(Settings.Default.ProxyUsernameEnc);
			settings.ProxyPassword = Enc.Decrypt(Settings.Default.ProxyPasswordEnc);

			return settings;
		}

		/// <summary>
		/// Static constructor to load current options
		/// </summary>
		static SettingsWrapper()
		{
			// Set the current version bumber
			CurrentVersion = Assembly.GetExecutingAssembly().GetName().Version;
		}

		public void Save()
		{
			string previousCulture = Settings.Default.PreferredCulture;

			Settings.Default.UsernameEnc = Enc.Encrypt(this.Username);
			Settings.Default.PasswordEnc = Enc.Encrypt(this.Password);
			Settings.Default.CheckIntervalMinutes = this.CheckIntervalMinutes;
			Settings.Default.WaveUrl = this.WaveUrl;
			Settings.Default.PlaySounds = this.PlaySounds;
			Settings.Default.SingleClickForcesCheck = this.SingleClickForcesCheck;
			Settings.Default.BalloonTimeoutSeconds = this.BalloonTimeout;
			Settings.Default.PreferredCulture = this.PreferredCulture;
			Settings.Default.PreferredBrowser = this.PreferredBrowser;
			Settings.Default.RunAtStartup = this.RunAtStartup;
			Settings.Default.ProxyAddress = this.ProxyAddress;
			Settings.Default.ProxyUsernameEnc = Enc.Encrypt(this.ProxyUsername);
			Settings.Default.ProxyPasswordEnc = Enc.Encrypt(this.ProxyPassword);
			Settings.Default.Save();

			// Set whether to run at startup
			Program.SetStartup(Settings.Default.RunAtStartup);

			// Restart the app so it loads with the new settings
			if (previousCulture != Settings.Default.PreferredCulture)
				Application.Restart();
		}

		public bool IsValid(out string error)
		{
			if (this.CheckIntervalMinutes < 1)
			{
				error = Languages.InterfaceText.CheckIntervalWarningText;
				return false;
			}

			if (this.Username != null && this.Username.ToLower().Contains("@googlewave"))
			{
				error = Languages.InterfaceText.UsernameWarningText;
				return false;
			}

			error = null;
			return true;
		}

		[ResourceCategory("LoginDetails")]
		[ResourceDisplayName("Username")]
		[ResourceDescription("UsernameDescription")]
		public string Username { get; set; }

		[ResourceCategory("LoginDetails")]
		[ResourceDisplayName("Password")]
		[ResourceDescription("PasswordDescription")]
		[PasswordPropertyTextAttribute(true)]
		public string Password { get; set; }

		[ResourceCategory("Notifications")]
		[ResourceDisplayName("CheckInterval")]
		[ResourceDescription("CheckIntervalDescription")]
		[DefaultValue(15)]
		public int CheckIntervalMinutes { get; set; }

		[ResourceCategory("Notifications")]
		[ResourceDisplayName("WaveUrl")]
		[ResourceDescription("WaveUrlDescription")]
		[DefaultValue("http://wave.google.com/")]
		public string WaveUrl { get; set; }

		[ResourceCategory("Notifications")]
		[ResourceDisplayName("PlaySounds")]
		[ResourceDescription("PlaySoundsDescription")]
		[DefaultValue(true)]
		public bool PlaySounds { get; set; }

		[ResourceCategory("Notifications")]
		[ResourceDisplayName("SingleClickCheck")]
		[ResourceDescription("SingleClickCheckDescription")]
		[DefaultValue(true)]
		public bool SingleClickForcesCheck { get; set; }

		[ResourceCategory("Notifications")]
		[ResourceDisplayName("BalloonTimeout")]
		[ResourceDescription("BalloonTimeoutDescription")]
		[DefaultValue(9)]
		public int BalloonTimeout { get; set; }

		[ResourceCategory("Application")]
		[ResourceDisplayName("PreferredCulture")]
		[ResourceDescription("PreferredCultureDescription")]
		[DefaultValue("Windows Default")]
		[TypeConverter(typeof(LanguageTypeConverter))]
		public string PreferredCulture { get; set; }

		[ResourceCategory("Application")]
		[ResourceDisplayName("PreferredBrowser")]
		[ResourceDescription("PreferredBrowserDescription")]
		[DefaultValue("Default Browser")]
		[TypeConverter(typeof(BrowserTypeConverter))]
		public string PreferredBrowser { get; set; }

		[ResourceCategory("Application")]
		[ResourceDisplayName("RunAtStartup")]
		[ResourceDescription("RunAtStartupDescription")]
		[DefaultValue(true)]
		public bool RunAtStartup { get; set; }

		[ResourceCategory("ProxyServer")]
		[ResourceDisplayName("ProxyAddress")]
		[ResourceDescription("ProxyAddressDescription")]
		public string ProxyAddress { get; set; }

		[ResourceCategory("ProxyServer")]
		[ResourceDisplayName("ProxyUsername")]
		[ResourceDescription("ProxyUsernameDescription")]
		public string ProxyUsername { get; set; }

		[ResourceCategory("ProxyServer")]
		[ResourceDisplayName("ProxyPassword")]
		[ResourceDescription("ProxyPasswordDescription")]
		[PasswordPropertyTextAttribute(true)]
		public string ProxyPassword { get; set; }
	}
}
