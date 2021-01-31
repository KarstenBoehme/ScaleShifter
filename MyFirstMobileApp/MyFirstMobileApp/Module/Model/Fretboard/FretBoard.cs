using MyFirstMobileApp.Module.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace MyFirstMobileApp.Module.Fretboard
{
	public enum GuitarString
	{
		E4 = 0,
		B = 1,
		G = 2,
		D = 3,
		A = 4,
		E = 5
	}

	public class FretBoard : IFretBoardLayoutProvider
	{
		public Tuning Tuning { get; private set; }
		public Scale Scale { get; private set; }
		public Key Key { get; private set; }
		public int CapoPosition { get; set; }
		public Dictionary<GuitarString, List<FretBoardPosition>> FretBoardLayout { get; private set; }

		private static readonly Dictionary<GuitarString, List<int>> OctaveLayout =
			new Dictionary<GuitarString, List<int>>()
			{
				{ GuitarString.E4, new List<int>()  {3, 3, 3, 3, 4, 4, 4, 4, /*E:*/ 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, 5, 5, /*E:*/ 5, 5, 5, 5, 5, 5, 5, 5, 6, 6, 6, 6, /*E:*/ 6, 6, 6, 6} },
				{ GuitarString.B, new List<int>()   {3, 3, 3, 3, 3, 3, 3, 3, /*B:*/ 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, /*B:*/ 4, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, /*B:*/ 5, 6, 6, 6} },
				{ GuitarString.G, new List<int>()   {2, 3, 3, 3, 3, 3, 3, 3, /*G:*/ 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, /*G:*/ 4, 4, 4, 4, 4, 5, 5, 5, 5, 5, 5, 5, /*G:*/ 5, 5, 5, 5} },
				{ GuitarString.D, new List<int>()   {2, 2, 2, 2, 2, 2, 3, 3, /*D:*/ 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, /*D:*/ 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 5, 5, /*D:*/ 5, 5, 5, 5} },
				{ GuitarString.A, new List<int>()   {2, 2, 2, 2, 2, 2, 2, 2, /*A:*/ 2, 2, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, /*A:*/ 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, /*A:*/ 4, 4, 4, 5} },
				{ GuitarString.E, new List<int>()   {1, 1, 1, 1, 2, 2, 2, 2, /*E:*/ 2, 2, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, /*E:*/ 3, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, /*E:*/ 4, 4, 4, 4} },
			};

		public FretBoard()
		{
			FretBoardLayout = new Dictionary<GuitarString, List<FretBoardPosition>>();

			InitilizeFretboardLayout();
		}

		private void InitilizeFretboardLayout()
		{
			EnumCollectionCreator<GuitarString>.GetEnumCollection().ToList()
				.ForEach(guitarString => FretBoardLayout.Add(guitarString, new List<FretBoardPosition>()));

			Keys.ListOfKeysForFretLayout().ForEach(e => FretBoardLayout[GuitarString.E4].Add(new FretBoardPosition(e)));
			Keys.ListOfKeysForFretLayout().ForEach(e => FretBoardLayout[GuitarString.B].Add(new FretBoardPosition(e)));
			Keys.ListOfKeysForFretLayout().ForEach(e => FretBoardLayout[GuitarString.G].Add(new FretBoardPosition(e)));
			Keys.ListOfKeysForFretLayout().ForEach(e => FretBoardLayout[GuitarString.D].Add(new FretBoardPosition(e)));
			Keys.ListOfKeysForFretLayout().ForEach(e => FretBoardLayout[GuitarString.A].Add(new FretBoardPosition(e)));
			Keys.ListOfKeysForFretLayout().ForEach(e => FretBoardLayout[GuitarString.E].Add(new FretBoardPosition(e)));
		}
		public void SetScale(Scale scale, Key key)
		{
			this.Scale = scale;
			this.Key = key;

			foreach (GuitarString stringKey in FretBoardLayout.Keys)
			{
				foreach (FretBoardPosition fretBoardPosition in FretBoardLayout[stringKey])
				{
					fretBoardPosition.IsScaleNote =
						Scale.GetScaleNotes(Key).Contains(fretBoardPosition.Key) ? true : false;

					fretBoardPosition.IsRootNote =
						fretBoardPosition.Key == Key ? true : false;

					fretBoardPosition.Interval = 
						Intervalls.GetInterval(this.Key, fretBoardPosition.Key);
				}
			}
		}
		public void SetTuning(Tuning tuning)
		{
			this.Tuning = (Tuning)tuning.Clone();

			foreach (GuitarString stringKey in FretBoardLayout.Keys)
			{
				Key newTuningKey = tuning.ToDict()[stringKey];
				UpdateOctaves(stringKey);

				while (FretBoardLayout[stringKey][0].Key != newTuningKey)
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
			UpdateOctaves(guitarString);
			UpdateFretboardTuning(guitarString);

		}
		public void TuneDown(GuitarString guitarString)
		{
			FretBoardPosition firstPos = this.FretBoardLayout[guitarString].First();
			this.FretBoardLayout[guitarString].RemoveAt(0);
			this.FretBoardLayout[guitarString].Add(firstPos);
			UpdateOctaves(guitarString);
			UpdateFretboardTuning(guitarString);
		}

		private void UpdateFretboardTuning(GuitarString guitarString)
		{
			switch (guitarString)
			{
				case GuitarString.E4:
					Tuning.E1 = FretBoardLayout[guitarString][0].Key.ToString();
					break;
				case GuitarString.B:
					Tuning.B = FretBoardLayout[guitarString][0].Key.ToString();
					break;
				case GuitarString.G:
					Tuning.G = FretBoardLayout[guitarString][0].Key.ToString();
					break;
				case GuitarString.D:
					Tuning.D = FretBoardLayout[guitarString][0].Key.ToString();
					break;
				case GuitarString.A:
					Tuning.A = FretBoardLayout[guitarString][0].Key.ToString();
					break;
				case GuitarString.E:
					Tuning.E = FretBoardLayout[guitarString][0].Key.ToString();
					break;
				default:
					throw new ArgumentException($"unhandled enum {typeof(GuitarString)}");
			}
		}

		private void UpdateOctaves(GuitarString guitarString)
		{
			List<FretBoardPosition> fretBoardPositionsOnString = this.FretBoardLayout[guitarString];
			int offsetFromStandard = GetStepsFromStandard(Keys.GetKey(guitarString), fretBoardPositionsOnString[0].Key);
			List<int> octaveRange = OctaveLayout[guitarString].Skip(Constants.MaxDownTuneSteps + offsetFromStandard).Take(Constants.NumberOfFrets).ToList();

			for (int i = 0; i < Constants.NumberOfFrets; i++)
			{
				fretBoardPositionsOnString[i].PianoOctave = octaveRange[i];
			}
		}

		private int GetStepsFromStandard(Key standardKey, Key tunedKey)
		{
			IEnumerable<Key> upperPart = Keys.ListOfKeys.GetRange(Keys.ListOfKeys.IndexOf(standardKey), Keys.ListOfKeys.Count() - Keys.ListOfKeys.IndexOf(standardKey));
			IEnumerable<Key> lowerPart = Keys.ListOfKeys.Except(upperPart);
			List<Key> listOfAllNotesReorderd = upperPart.Concat(lowerPart).ToList();

			int offset = listOfAllNotesReorderd.IndexOf(tunedKey);

			return offset <= Constants.MaxUpTuneSteps ? offset : offset - listOfAllNotesReorderd.Count;
		}
	}
}
