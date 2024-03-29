﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMobileApp.Module
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
		INTERVAL
	}

	public enum FretboardOrientationSettings
	{
		RIGHT_HAND,
		LEFT_HAND,
	}

	public static class Settings
	{
		public static KeyDisplayingSettings KeyDisplayingSettings { get; set;}
		public static SemiStepSettings SemiStepSettings { get; set; }
		public static FretboardOrientationSettings FretboardOrientationSettings { get; set; }
	}
}
