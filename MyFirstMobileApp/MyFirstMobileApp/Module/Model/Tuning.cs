using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstMobileApp
{
	[Table("tunings")]
	public class Tuning : ICloneable
	{
		[PrimaryKey, AutoIncrement]
		[Column("index")]
		public int Index { get; set; }

		[Column("description")]
		public string Description { get; set; }

		[Column("locked")]
		public int locked { get; set; }

		[Column("E1")]
		public string E1 { get; set; }

		[Column("B")]
		public string B { get; set; }

		[Column("G")]
		public string G { get; set; }

		[Column("D")]
		public string D { get; set; }

		[Column("A")]
		public string A { get; set; }

		[Column("E")]
		public string E { get; set; }

		public string Notes
		{
			get
			{
				return GetNotes();
			}
		}

		public Dictionary<GuitarString, Key> ToDict()
		{
			Dictionary<GuitarString, Key> tuning = new Dictionary<GuitarString, Key>
			{
				{ GuitarString.E1, Keys.ListOfKeys.Single(k => k.ToString().Equals(E1))},
				{ GuitarString.B, Keys.ListOfKeys.Single(k => k.ToString().Equals(B))},
				{ GuitarString.G, Keys.ListOfKeys.Single(k => k.ToString().Equals(G))},
				{ GuitarString.D, Keys.ListOfKeys.Single(k => k.ToString().Equals(D))},
				{ GuitarString.A, Keys.ListOfKeys.Single(k => k.ToString().Equals(A))},
				{ GuitarString.E, Keys.ListOfKeys.Single(k => k.ToString().Equals(E))},
			};
			return tuning;
		}

		private string GetNotes()
		{
			var thisTuning = ToDict();

			string fullDescription =
			thisTuning[GuitarString.E].GetKeyDiscription() +
			thisTuning[GuitarString.A].GetKeyDiscription() +
			thisTuning[GuitarString.D].GetKeyDiscription() +
			thisTuning[GuitarString.G].GetKeyDiscription() +
			thisTuning[GuitarString.B].GetKeyDiscription() +
			thisTuning[GuitarString.E1].GetKeyDiscription();

			return fullDescription;
		}


		public bool IsLocked()
		{
			return locked == 1;
		}

		public object Clone()
		{
			Tuning clone = (Tuning)this.MemberwiseClone();
			clone.locked = 0;

			return clone;
		}
	}
}
