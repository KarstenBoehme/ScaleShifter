using MyFirstMobileApp.Module.Fretboard;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstMobileApp.Module.Properties
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
		
		public static List<Key> ListOfKeysForFretLayout()
		{
			List<Key> listOfKeysForFretLayout = ListOfKeys;

			while(Constants.NumberOfFrets - listOfKeysForFretLayout.Count > 0)
			{
				listOfKeysForFretLayout.AddRange(ListOfKeys);
			}

			return listOfKeysForFretLayout.Take(Constants.NumberOfFrets).ToList();
		}

		public static Key GetKey(GuitarString guitarString)
		{
			switch (guitarString)
			{
				case GuitarString.E4:
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
	}
}
