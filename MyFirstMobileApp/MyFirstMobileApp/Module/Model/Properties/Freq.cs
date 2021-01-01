using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMobileApp.Module.Properties
{
	[Table("Freq")]
	public class Freq
	{
		[PrimaryKey, AutoIncrement]
		[Column("index")]
		public int Index { get; set; }

		[Column("note")]
		public string Note { get; set; }

		[Column("octave")]
		public int Octave { get; set; }

		[Column("freq")]
		public double Frequency { get; set; }

		[Column("min_freq")]
		public double MinFrequency { get; set; }

		[Column("max_freq")]
		public double MaxFrequency { get; set; }

		public bool IsWithinPlusMinus50Cent(double value)
		{
			return value > MinFrequency && value <= MaxFrequency;
		}
	}
}
