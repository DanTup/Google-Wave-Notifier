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
using System.Diagnostics;
using GoogleWaveNotifier.Properties;
using System.Windows.Forms;

namespace GoogleWaveNotifier
{
	/// <summary>
	/// Browser utility class
	/// </summary>
	static class WebBrowser
	{
		/// <summary>
		/// Launch a URL
		/// </summary>
		/// <param name="url"></param>
		public static void Launch(string url)
		{
			Launch(url, false);
		}

		/// <summary>
		/// Launch a URL
		/// </summary>
		/// <param name="url"></param>
		/// <param name="allowChromeAppMode"></param>
		public static void Launch(string url, bool allowChromeAppMode)
		{
			string browser = null;

			// Grab the prefered browser location
			if (BrowserTypeConverter.Browsers.ContainsKey(Settings.Default.PreferredBrowser))
				browser = BrowserTypeConverter.Browsers[Settings.Default.PreferredBrowser];

			bool chromeAppMode = false;
			if (browser != null && browser.StartsWith("APP:")) // HACK: Chrome Application Mode
			{
				browser = browser.Substring(4); // Strip off the "APP: hack
				chromeAppMode = true;
			}

			try
			{
				// If no browser, launch default
				if (browser == null)
					Process.Start(url);
				else if (allowChromeAppMode && chromeAppMode) // If chrome, and wanted app mode
					Process.Start(browser, "--app=" + url);
				else
					Process.Start(browser, url);
			}
			catch (Exception ex)
			{
				// Log errors if we couldn't launch the browser and alert the user
				Log.Error(ex);
				MessageBox.Show(string.Format("The selected browser, '{0}', could not be launched.", browser), "Couldn't find Browser");
			}
		}
	}
}
