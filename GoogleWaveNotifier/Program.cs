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
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using GoogleWaveNotifier.Properties;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.Diagnostics;

namespace GoogleWaveNotifier
{
	static class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// Hook global errors
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

			// Upgrade settings if required
			UpgradeSettingsIfRequired();

			// Set the UI language
			SetPreferredCulture();

			// Load the form
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new GoogleWaveNotifier());
		}

		static void UpgradeSettingsIfRequired()
		{
			// If we're a new version, we'll have lost all settings, so pull
			// in the previous settings
			if (!Settings.Default.HasImportedPreviousSettings)
			{
				Settings.Default.Upgrade();
				Settings.Default.HasImportedPreviousSettings = true;
				Settings.Default.Save();
			}

			// Make sure startup entry is set correctly
			SetStartup(Settings.Default.RunAtStartup);
		}

		static void SetPreferredCulture()
		{
			// Get the culture
			string wantedCulture = null;
			if (LanguageTypeConverter.Languages.ContainsKey(Settings.Default.PreferredCulture))
				wantedCulture = LanguageTypeConverter.Languages[Settings.Default.PreferredCulture];

			if (wantedCulture != null)
				Thread.CurrentThread.CurrentUICulture = new CultureInfo(wantedCulture);
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			// Log all unhandled exceptions
			Exception ex = e.ExceptionObject as Exception;
			if (ex != null)
			{
				Log.Error(ex);
				if (ex is FileNotFoundException)
					MessageBox.Show(Languages.InterfaceText.UnknownErrorDescriptionDotNet, Languages.InterfaceText.UnknownError);
				else
					MessageBox.Show(Languages.InterfaceText.UnknownErrorDescription, Languages.InterfaceText.UnknownError);
			}
			else
			{
				MessageBox.Show("Something went wrong, but no exception was provided. Please file a bug!", Languages.InterfaceText.UnknownError);
			}
		}

		/// <summary>
		/// Set the app to run (or not) at startup
		/// </summary>
		/// <param name="runAtStartup">Whether to run the app at startup</param>
		public static void SetStartup(bool runAtStartup)
		{
			// Taken from here:
			// http://vbaccelerator.com/home/NET/Code/Libraries/Shell_Projects/Creating_and_Modifying_Shortcuts/article.asp

			string shortcutFileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), "Google Wave Notifier.lnk");
			if (runAtStartup)
			{
				// Always replace the shortcut, in case it's an old version, in a different location
				if (File.Exists(shortcutFileName))
					File.Delete(shortcutFileName);
				
				ShellLink shortcut = new ShellLink();
				shortcut.Target = Application.ExecutablePath;
				shortcut.WorkingDirectory = Path.GetDirectoryName(Application.ExecutablePath);
				shortcut.Description = "Google Wave Notifier by Danny Tuppeny";
				shortcut.DisplayMode = ShellLink.LinkDisplayMode.edmMinimized;
				shortcut.Save(shortcutFileName);
			}
			else
			{
				// Remove shortcut if exists
				if (File.Exists(shortcutFileName))
				{
					try
					{
						File.Delete(shortcutFileName);
					}
					catch (Exception ex)
					{
						Log.Error(ex);
					}
				}
			}
		}
	}
}