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

namespace GoogleWaveNotifier
{
	class LanguageTypeConverter : TypeConverter
	{
		/// <summary>
		/// A list of all supported languages (key = name, value = culture code)
		/// </summary>
		public static Dictionary<string, string> Languages { get; private set; }

		static LanguageTypeConverter()
		{
			// Populate the list with completed languages
			Languages = new Dictionary<string, string>();
			Languages.Add("Windows Default", null);
			Languages.Add("English (en-GB)", "en-GB");
			Languages.Add("Brazilian Portuguese (pt-BR)", "pt-BR");
			Languages.Add("Czech (cs-CZ)", "cs-CZ");
			Languages.Add("Danish (da-DK)", "da-DK");
			Languages.Add("Dutch (nl-NL)", "nl-NL");
			Languages.Add("French (fr-FR)", "fr-FR");
			Languages.Add("German (de-DE)", "de-DE");
			//Languages.Add("Hebrew (he-IL)", "he-IL");
			Languages.Add("Hungarian (hu-HU)", "hu-HU");
			Languages.Add("Italian (it-IT)", "it-IT");
			Languages.Add("Polish (pl-PL)", "pl-PL");
			Languages.Add("Romanian (ro-RO)", "ro-RO");
			Languages.Add("Russian (ru-RU)", "ru-RU");
			Languages.Add("Spanish (es-ES)", "es-ES");
			Languages.Add("Swedish (sv-SE)", "sv-SE");
			Languages.Add("Turkish (tr-TR)", "tr-TU");
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
			return new StandardValuesCollection(Languages.Keys);
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			return GetValues();
		}

		#endregion
	}
}
