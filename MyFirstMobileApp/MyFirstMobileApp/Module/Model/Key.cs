using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyFirstMobileApp
{
	public enum Key
	{
		C,
		CSHARP,
		D,
		DSHARP,
		E,
		F,
		FSHARP,
		G,
		GSHARP,
		A,
		ASHARP,
		B
	}
	public static class Keys
	{
		public static List<Key> ListOfKeys => EnumCollectionCreator<Key>.GetEnumCollection().ToList();

		public static Key GetKey(GuitarString guitarString)
		{
			switch (guitarString)
			{
				case GuitarString.E1:
				case GuitarString.E:
					return Key.E;

				case GuitarString.B:
					return Key.B;

				case GuitarString.G:
					return Key.G;

				case GuitarString.D:
					return Key.D;

				case GuitarString.A:
					return Key.A;

				default:
					throw new ArgumentException($"unhandled enum: {nameof(GuitarString)}");
			}
		}

		public static Key GetKey(string guitarString)
		{
			return ListOfKeys.Single(k => k.ToString().Equals(guitarString));
		}

		public static string GetKeyDiscription(this Key key)
		{
			bool isSharp = Settings.SemiStepSettings == SemiStepSettings.SHARP;

			switch (key)
			{
				case Key.C:
					return "C";

				case Key.CSHARP:
					return isSharp ? "C♯" : "D♭";
				
				case Key.D:
					return "D";

				case Key.DSHARP:
					return isSharp ? "D♯" : "E♭";

				case Key.E:
					return "E";

				case Key.F:
					return "F";

				case Key.FSHARP:
					return isSharp ? "F♯" : "G♭";

				case Key.G:
					return "G";

				case Key.GSHARP:
					return isSharp ? "G♯" : "A♭";

				case Key.A:
					return "A";

				case Key.ASHARP:
					return isSharp ? "A♯" : "B♭";

				case Key.B:
					return "B";

				default:
					throw new ArgumentException($"unhandled enum: {nameof(Key)}");
			}
		}

		public static Key OffsetCapo(this Key key, int capoPosition)
		{
			int indexKey = Keys.ListOfKeys.IndexOf(key);
			int indexNewKeyWithCapo = indexKey + capoPosition;
			int lastIndex = Keys.ListOfKeys.Count() - 1;

			bool isOutOfRange = indexNewKeyWithCapo > lastIndex;
			if (isOutOfRange)
			{
				indexNewKeyWithCapo = Math.Abs(indexNewKeyWithCapo - lastIndex - 1);
			}

			Key offsetKey = Keys.ListOfKeys[indexNewKeyWithCapo];

			return offsetKey;
		}

		public static Key BaseForCapoOffset(this Key key, int capoPosition)
		{
			int indexKey = Keys.ListOfKeys.IndexOf(key);
			int indexNewKeyWithCapo = indexKey - capoPosition;
			int lastIndex = Keys.ListOfKeys.Count() - 1;

			bool isOutOfRange = indexNewKeyWithCapo < 0;
			if (isOutOfRange)
			{
				indexNewKeyWithCapo = Math.Abs(lastIndex + indexNewKeyWithCapo + 1);
			}

			Key offsetKey = Keys.ListOfKeys[indexNewKeyWithCapo];

			return offsetKey;
		}
	}
}
