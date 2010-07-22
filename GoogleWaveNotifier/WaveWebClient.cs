using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using GoogleWaveNotifier.Properties;

namespace GoogleWaveNotifier
{
	class WaveWebClient : WebClient
	{
		public CookieContainer CookieContainer = new CookieContainer();

		public WaveWebClient()
			: base()
		{
			this.Encoding = Encoding.UTF8;

			if (!string.IsNullOrEmpty(Settings.Default.ProxyAddress))
			{
				var proxy = new WebProxy(Settings.Default.ProxyAddress);

				if (!string.IsNullOrEmpty(Enc.Decrypt(Settings.Default.ProxyUsernameEnc)))
				{
					proxy.Credentials = new NetworkCredential(Enc.Decrypt(Settings.Default.ProxyUsernameEnc), Enc.Decrypt(Settings.Default.ProxyPasswordEnc));
				}
				else
					proxy.UseDefaultCredentials = true;

				this.Proxy = proxy;
			}
		}

		protected override WebRequest GetWebRequest(Uri address)
		{
			WebRequest request = base.GetWebRequest(address);
			if (request is HttpWebRequest)
			{
				(request as HttpWebRequest).CookieContainer = this.CookieContainer;
			}
			return request;
		}
	}
}
