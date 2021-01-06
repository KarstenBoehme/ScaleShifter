using MyFirstMobileApp.Module.Fretboard;
using MyFirstMobileApp.Module.Properties;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;

namespace MyFirstMobileApp.Module
{
	public static class UiDescriptionProvider
	{
		public static string GetStepsFromStandard(this Key standardKey, Key tunedKey)
		{
			IEnumerable<Key> upperPart = Keys.ListOfKeys.GetRange(Keys.ListOfKeys.IndexOf(standardKey), Keys.ListOfKeys.Count() - Keys.ListOfKeys.IndexOf(standardKey));
			IEnumerable<Key> lowerPart = Keys.ListOfKeys.Except(upperPart);

			List<Key> listOfAllNotesReorderd = upperPart.Concat(lowerPart).ToList();

			if (listOfAllNotesReorderd.IndexOf(tunedKey) == 0)
			{
				return "±0";
			}
			else if (listOfAllNotesReorderd.IndexOf(tunedKey) <= Constants.MaxUpTuneSteps)
			{
				return $"+{listOfAllNotesReorderd.IndexOf(tunedKey)}";
			}
			else
			{
				return $"-{listOfAllNotesReorderd.Count - listOfAllNotesReorderd.IndexOf(tunedKey)}";
			}
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

		public static IEnumerable<Color> GetGradientedRedGreen(int steps)
		{
			double rMax = Color.Green.R;
			double rMin = Color.Red.R;

			double gMax = Color.Green.G;
			double gMin = Color.Red.G;

			var colorList = new List<Color>();

			for (int i = 0; i < steps; i++)
			{
				var rAverage = rMin + (int)((rMax - rMin) * i / steps);
				var gAverage = gMin + (int)((gMax - gMin) * i / steps);
				var bAverage = 0;
				
				colorList.Add(Color.FromRgb(rAverage, gAverage, bAverage));
			}

			return colorList;
		}
	}
}
