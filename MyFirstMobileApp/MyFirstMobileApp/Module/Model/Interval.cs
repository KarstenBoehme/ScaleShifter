using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyFirstMobileApp
{
	public enum Interval
	{
		P1,
		m2,
		M2,
		m3,
		M3,
		P4,
		TT,
		P5,
		m6,
		M6,
		m7,
		M7,
		P8
	}
	public static class Intervalls
	{
		public static Interval GetInterval(Key root, Key note)
		{
			IEnumerable<Key> upperPart = Keys.ListOfKeys.GetRange(Keys.ListOfKeys.IndexOf(root), Keys.ListOfKeys.Count() - Keys.ListOfKeys.IndexOf(root));
			IEnumerable<Key> lowerPart = Keys.ListOfKeys.Except(upperPart);
			List<Key> listOfAllNotesReordered = upperPart.Concat(lowerPart).ToList();

			int indexOfNote = listOfAllNotesReordered.IndexOf(note);
			Interval interval = EnumCollectionCreator<Interval>.GetEnumCollection().ToList()[indexOfNote];
			
			return interval;
		}
	}
}
