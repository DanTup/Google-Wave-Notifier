using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace GoogleWaveNotifier
{
	/// <summary>
	/// Support for reading property grid text fom resource files
	/// </summary>
	class ResourceCategoryAttribute : CategoryAttribute
	{
		public ResourceCategoryAttribute(string text)
			: base(text)
		{
		}

		protected override string GetLocalizedString(string value)
		{
			return Languages.InterfaceText.ResourceManager.GetString(value);
		}
	}

	/// <summary>
	/// Support for reading property grid text fom resource files
	/// </summary>
	class ResourceDescriptionAttribute : DescriptionAttribute
	{
		public ResourceDescriptionAttribute(string text)
			: base(text)
		{
		}

		public override string Description
		{
			get
			{
				return Languages.InterfaceText.ResourceManager.GetString(this.DescriptionValue);
			}
		}
	}

	/// <summary>
	/// Support for reading property grid text fom resource files
	/// </summary>
	class ResourceDisplayNameAttribute : DisplayNameAttribute
	{
		public ResourceDisplayNameAttribute(string text)
			: base(text)
		{
		}

		public override string DisplayName
		{
			get
			{
				return Languages.InterfaceText.ResourceManager.GetString(this.DisplayNameValue);
			}
		}
	}
}
