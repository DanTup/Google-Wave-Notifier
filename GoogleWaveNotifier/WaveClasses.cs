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
using System.Runtime.Serialization;

namespace GoogleWaveNotifier
{
	// These classes match the Json output by Google. I've only included the
	// properties we care about for what we need to avoid breaking on other changes.
	// We may need to update this if Google change things.

	[DataContract]
	class WaveData
	{
		[DataMember(Name = "p")]
		public WaveFolder Inbox { get; set; }
	}

	[DataContract]
	class WaveFolder
	{
		[DataMember(Name = "1")]
		public Wave[] Waves { get; set; }
	}

	[DataContract]
	class Wave
	{
		[DataMember(Name = "6")]
		public int TotalMessages { get; set; }

		[DataMember(Name = "7")]
		public int NewMessages { get; set; }

		[DataMember(Name = "9")]
		public WaveMessage OriginalMessage { get; set; }
	}

	[DataContract]
	class WaveMessage
	{
		[DataMember(Name = "1")]
		public string Text { get; set; }
	}
}
