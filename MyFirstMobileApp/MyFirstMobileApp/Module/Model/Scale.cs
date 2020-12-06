using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyFirstMobileApp
{
	[Table("scales")]
	public class Scale
	{
		[PrimaryKey, AutoIncrement]
		[Column("index")]
		public int Index { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("steps")]
		public int Steps { get; set; }

		public IEnumerable<Key> GetScaleNotes(Key root)
		{
			List<Key> scaleNotes = new List<Key>();

			IEnumerable<Key> upperPart = Keys.ListOfKeys.GetRange(Keys.ListOfKeys.IndexOf(root), Keys.ListOfKeys.Count() - Keys.ListOfKeys.IndexOf(root));
			IEnumerable<Key> lowerPart = Keys.ListOfKeys.Except(upperPart);

			List<Key> listOfAllNotesReorderd = upperPart.Concat(lowerPart).ToList();

			int indexOfNote = 0;
			IEnumerable<int> steps = Steps.ToString().ToCharArray().Select(c => (int)Char.GetNumericValue(c));

			foreach (int step in steps)
			{
				scaleNotes.Add(listOfAllNotesReorderd[indexOfNote]);
				indexOfNote += step;
			}

			return scaleNotes;
		}
	}
}
