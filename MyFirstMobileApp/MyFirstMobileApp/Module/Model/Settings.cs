using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMobileApp
{
	public enum KeyDisplayingSettings
	{
		ALL,
		SCALE,
		NONE
	}

	public enum SemiStepSettings
	{
		SHARP,
		FLAT,
	}

	public static class Settings
	{
		public static KeyDisplayingSettings KeyDisplayingSettings { get; set;}
		public static SemiStepSettings SemiStepSettings { get; set; }
	}
}
