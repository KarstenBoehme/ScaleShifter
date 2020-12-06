using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace MyFirstMobileApp
{
	public enum GuitarString
	{
		E1 = 0,
		B = 1,
		G = 2,
		D = 3,
		A = 4,
		E = 5
	}

	public class FretBoard
	{
		public Tuning Tuning { get; set; }
		public Scale Scale { get; set; }
		public Key Key { get; set; }
		public int CapoPosition { get; set; }
		public Dictionary<GuitarString, List<FretBoardPosition>> FretBoardLayout { get; set; }

		public FretBoard()
		{		
			FretBoardLayout = new Dictionary<GuitarString, List<FretBoardPosition>>();

			EnumCollectionCreator<GuitarString>.GetEnumCollection().ToList()
				.ForEach(gs => FretBoardLayout.Add(gs, new List<FretBoardPosition>()));

			Keys.ListOfKeys.ForEach(e => FretBoardLayout[GuitarString.E1].Add(new FretBoardPosition(e)));
			Keys.ListOfKeys.ForEach(e => FretBoardLayout[GuitarString.B].Add(new FretBoardPosition(e)));
			Keys.ListOfKeys.ForEach(e => FretBoardLayout[GuitarString.G].Add(new FretBoardPosition(e)));
			Keys.ListOfKeys.ForEach(e => FretBoardLayout[GuitarString.D].Add(new FretBoardPosition(e)));
			Keys.ListOfKeys.ForEach(e => FretBoardLayout[GuitarString.A].Add(new FretBoardPosition(e)));
			Keys.ListOfKeys.ForEach(e => FretBoardLayout[GuitarString.E].Add(new FretBoardPosition(e)));
		}

		public void UpdateScale()
		{
			foreach (GuitarString stringKey in FretBoardLayout.Keys)
			{
				foreach (FretBoardPosition fretBoardPosition in FretBoardLayout[stringKey])
				{
					fretBoardPosition.IsScaleNote =
						Scale.GetScaleNotes(Key).Contains(fretBoardPosition.Key) ? true : false;

					fretBoardPosition.IsRootNote =
						fretBoardPosition.Key == Key ? true : false;
				}
			}
		}

		public void UpdateTuning(Tuning tuning)
		{
			Tuning = (Tuning)tuning.Clone();
			TuneFretboard(tuning);
		}

		public void RefreshTuning()
		{
			TuneFretboard(Tuning);
		}

		private void TuneFretboard(Tuning tuning)
		{
			foreach (GuitarString stringKey in FretBoardLayout.Keys)
			{
				Key tuningKey = tuning.ToDict()[stringKey];
				Key offsetKey = tuningKey.OffsetCapo(CapoPosition);

				while (FretBoardLayout[stringKey][0].Key != offsetKey)
				{
					TuneUp(stringKey);
				}
			}
		}

		public void TuneUp(GuitarString guitarString)
		{
			FretBoardPosition lastPos = this.FretBoardLayout[guitarString].Last();
			this.FretBoardLayout[guitarString].Remove(lastPos);
			this.FretBoardLayout[guitarString].Insert(0, lastPos);
			TuneString(guitarString);
		}

		public void TuneDown(GuitarString guitarString)
		{
			FretBoardPosition firstPos = this.FretBoardLayout[guitarString].First();
			this.FretBoardLayout[guitarString].RemoveAt(0);
			this.FretBoardLayout[guitarString].Add(firstPos);
			TuneString(guitarString);
		}

		private void TuneString(GuitarString guitarString)
		{
			switch (guitarString)
			{
				case GuitarString.E1:
					Tuning.E1 = FretBoardLayout[guitarString][0].Key.BaseForCapoOffset(CapoPosition).ToString();
					break;
				case GuitarString.B:
					Tuning.B = FretBoardLayout[guitarString][0].Key.BaseForCapoOffset(CapoPosition).ToString();
					break;
				case GuitarString.G:
					Tuning.G = FretBoardLayout[guitarString][0].Key.BaseForCapoOffset(CapoPosition).ToString();
					break;
				case GuitarString.D:
					Tuning.D = FretBoardLayout[guitarString][0].Key.BaseForCapoOffset(CapoPosition).ToString();
					break;
				case GuitarString.A:
					Tuning.A = FretBoardLayout[guitarString][0].Key.BaseForCapoOffset(CapoPosition).ToString();
					break;
				case GuitarString.E:
					Tuning.E = FretBoardLayout[guitarString][0].Key.BaseForCapoOffset(CapoPosition).ToString();
					break;
				default:
					throw new ArgumentException($"unhandled enum {typeof(GuitarString)}");
			}
		}

		public string GetTuningDescription()
		{
			var tuningDict = Tuning.ToDict();

			return tuningDict[GuitarString.E].GetKeyDiscription() +
					tuningDict[GuitarString.A].GetKeyDiscription() +
					tuningDict[GuitarString.D].GetKeyDiscription() +
					tuningDict[GuitarString.G].GetKeyDiscription() +
					tuningDict[GuitarString.B].GetKeyDiscription() +
					tuningDict[GuitarString.E1].GetKeyDiscription();
		}

		public string GetStepsFromStandard(GuitarString guitarString)
		{
			Key standardKey = Keys.GetKey(guitarString);
			Key tunedKey = FretBoardLayout[guitarString][0].Key.BaseForCapoOffset(CapoPosition);

			IEnumerable<Key> upperPart = Keys.ListOfKeys.GetRange(Keys.ListOfKeys.IndexOf(standardKey), Keys.ListOfKeys.Count() - Keys.ListOfKeys.IndexOf(standardKey));
			IEnumerable<Key> lowerPart = Keys.ListOfKeys.Except(upperPart);

			List<Key> listOfAllNotesReorderd = upperPart.Concat(lowerPart).ToList();

			if (listOfAllNotesReorderd.IndexOf(tunedKey) == 0)
			{
				return "±0";
			}
			else if(listOfAllNotesReorderd.IndexOf(tunedKey) <= 4)
			{
				return $"+{listOfAllNotesReorderd.IndexOf(tunedKey)}";
			} 
			else
			{
				return $"-{listOfAllNotesReorderd.Count - listOfAllNotesReorderd.IndexOf(tunedKey)}";
			}
		}
	}
}
