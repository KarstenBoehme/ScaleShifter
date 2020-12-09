using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMobileApp
{
	public static class Constants
	{
		public static string UpSign => Char.ConvertFromUtf32(9650).ToString();
		public static string DownSign => Char.ConvertFromUtf32(9660).ToString();

		public static int NumberOfStrings = 6;
		public static int NumberOfNotes = 12;

		public static string StandardTuningNotes = "EADGBE";
	}
}
