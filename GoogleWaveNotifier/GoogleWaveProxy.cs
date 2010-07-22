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
using System.Collections.Specialized;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;

namespace GoogleWaveNotifier
{
	class GoogleWaveProxy
	{
		NameValueCollection values = new NameValueCollection();

		// App name passed to Google
		string appName = "DannyTuppeny-WaveNotifier";

		public GoogleWaveProxy(string username, string password)
		{
			// Some params passed to Google during login
			values.Add("accountType", "GOOGLE");
			values.Add("service", "wave");
			values.Add("Email", username);
			values.Add("Passwd", password);
			values.Add("source", appName);

			// Set version number
			appName += "-v" + Assembly.GetExecutingAssembly().GetName().Version.ToString(2);

			// To allow Fiddler to intercept and show SSL requests, we need to stop
			// its invalid certificate causing an error
			//try
			//{
			//    //Change SSL checks so that all checks pass
			//    System.Net.ServicePointManager.ServerCertificateValidationCallback =
			//        new System.Net.Security.RemoteCertificateValidationCallback(
			//            delegate
			//            { return true; }
			//        );
			//}
			//catch (Exception ex)
			//{
			//    System.Windows.Forms.MessageBox.Show(ex.ToString());
			//}
		}

		/// <summary>
		/// Gets the Google Wave Inbox from the server
		/// </summary>
		/// <returns></returns>
		public WaveFolder GetInbox()
		{
			string authToken = null, waveToken = null;

			using (WaveWebClient client = new WaveWebClient())
			{
				// HACK: Send the login request. There's no nice API support, so we just
				// have to pretend to be a user.
				client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
				client.Headers.Add("User-Agent", appName);
				byte[] resultsBytes;
				try
				{
					resultsBytes = client.UploadValues("https://www.google.com/accounts/ClientLogin", values);
				}
				catch (Exception ex)
				{
					throw new LoginFailedException("Exception sending request", ex);
				}

				// Convert the response to a string
				string results = Encoding.ASCII.GetString(resultsBytes);

				// Check it was successful
				if (!results.Contains("Auth="))
					throw new LoginFailedException("Didn't find Auth= token");

				// Parse out the Auth token
				int position = results.IndexOf("Auth=") + 5;
				authToken = results.Substring(position);
				if (authToken.Contains("\n"))
					authToken = authToken.Substring(0, authToken.IndexOf("\n"));

				// Parse out the Wave token (not sure why we need this, but it doesn't work if not passed)
				position = results.IndexOf("SID=") + 4;
				waveToken = results.Substring(position);
				if (waveToken.Contains("\n"))
					waveToken = waveToken.Substring(0, waveToken.IndexOf("\n"));

				// Now send a request for the inbox
				client.Headers.Add("User-Agent", appName);
				// HACK: WebClient doesn't seem to pass the cookies (domain seems to be ".wave.google.com")
				// so we have to hack it in manually
				client.CookieContainer.SetCookies(new Uri("https://wave.google.com/"), "WAVE=" + authToken);

				// Grab inbox
				string inbox = client.DownloadString("https://wave.google.com/wave/?nouacheck&auth=" + HttpUtility.UrlEncode(authToken));

				// Try to find the inbox in the Json
				int jsonStartPos = inbox.IndexOf("var json = {\"r\":\"^d1\"");
				if (jsonStartPos == -1)
					throw new InvalidInboxResponseException("Unable to find \"var json = {\"r\":\"^d1\"\"");

				// Decode the Json using the classes we've built that match the bits we're interested in
				try
				{
					var json = inbox.Substring(jsonStartPos + 11);
					json = json.Substring(0, json.IndexOf("\n") - 1);

					using (MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
					{
						DataContractJsonSerializer serialiser = new DataContractJsonSerializer(typeof(WaveData));

						WaveData data = serialiser.ReadObject(ms) as WaveData;

						return data.Inbox;
					}
				}
				catch (Exception ex)
				{
					throw new InvalidJsonException("Error parsing Json", ex);
				}
			}
		}
	}
}
