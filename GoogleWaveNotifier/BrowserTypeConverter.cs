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
using System.ComponentModel;
using Microsoft.Win32;

namespace GoogleWaveNotifier
{
	class BrowserTypeConverter : TypeConverter
	{
		/// <summary>
		/// Dictionary of supported browsers. Key = name, value = path
		/// </summary>
		public static Dictionary<string, string> Browsers { get; private set; }

		static BrowserTypeConverter()
		{
			// Populate the browser list
			Browsers = new Dictionary<string, string>();
			// Support default system browser
			Browsers.Add("Default Browser", null);
			string path;

			// Internet Explorer
			if ((path = GetBrowserPath("iexplore")) != null)
			{
				Browsers.Add("Internet Explorer", "iexplore");
			}

			// Google Chrome
			if ((path = GetBrowserPath("chrome")) != null)
			{
				Browsers.Add("Google Chrome", path);
				Browsers.Add("Google Chrome (App Mode)", "APP:" + path); // HACK: Chrome Application Mode
			}

			// Firefox
			if ((path = GetBrowserPath("firefox")) != null)
			{
				Browsers.Add("Mozilla Firefox", path);
			}

			// Opera
			if ((path = GetBrowserPath("opera")) != null)
			{
				Browsers.Add("Opera", path);
			}
		}

		/// <summary>
		/// Static list (cache) of installed browsers to avoid looking them up multiple times
		/// </summary>
		static List<string> installedBrowsers;

		/// <summary>
		/// Gets the path of a specified browser
		/// </summary>
		/// <param name="browser"></param>
		/// <returns></returns>
		private static string GetBrowserPath(string browser)
		{
			try
			{
				// Grab all the browsers from the registry the first time we're called
				if (installedBrowsers == null)
				{
					installedBrowsers = new List<string>();
					AddBrowsers(@"SOFTWARE\Wow6432Node\Clients\StartMenuInternet");
					AddBrowsers(@"SOFTWARE\Clients\StartMenuInternet");
				}

				// Loop through checking for one that matches the requested browser
				foreach (string app in installedBrowsers)
				{
					if (app != null && app.ToLower().Contains(browser.ToLower()))
					{
						// Find preferred one, check 64bit first
						string browserPath;
						if ((browserPath = GetBrowserPath(app, @"SOFTWARE\Wow6432Node\Clients\StartMenuInternet\{0}\shell\open\command")) != null)
							return browserPath;
						if ((browserPath = GetBrowserPath(app, @"SOFTWARE\Clients\StartMenuInternet\{0}\shell\open\command")) != null)
							return browserPath;
					}
				}

				return null;
			}
			// We can't blow up during the check. There are many reasons we might generate errors here.
			// Just log them to the logfile and continue.
			catch (Exception ex)
			{
				Log.Error(ex);
				return null; // Just show all if errors occur
			}
		}

		/// <summary>
		/// Attempts to read a browser path from the registry.
		/// </summary>
		/// <param name="app"></param>
		/// <param name="location"></param>
		/// <returns></returns>
		private static string GetBrowserPath(string app, string location)
		{
			try
			{
				return Registry.LocalMachine.OpenSubKey(string.Format(location, app)).GetValue("").ToString();
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		/// Adds browsers from the specified registry location into our list. Wrapped with try/catch so we don't fail
		/// on security errors or keys not existing.
		/// </summary>
		/// <param name="location"></param>
		static void AddBrowsers(string location)
		{
			try
			{
				installedBrowsers.AddRange(Registry.LocalMachine.OpenSubKey(location).GetSubKeyNames());
			}
			catch { }
		}

		#region Boilerplate TypeConverter Stuff

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
		{
			return true;
		}

		private StandardValuesCollection GetValues()
		{
			return new StandardValuesCollection(Browsers.Keys);
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return GetValues();
		}

		#endregion
	}
}
