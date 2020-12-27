using MyFirstMobileApp.Module;
using MyFirstMobileApp.Module.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using MyFirstMobileApp.Module;

namespace MyFirstMobileApp.Module.Fretboard
{
	public class FredBoardGrid
	{
		private FastFixedSizeLabel[][] dataArray = new FastFixedSizeLabel[7][];
		public Grid UIGrid { get; private set; }
		private IFretBoardLayoutProvider FretBoardLayoutProvider { get; }

		public FredBoardGrid(IFretBoardLayoutProvider fretBoard)
		{
			FretBoardLayoutProvider = fretBoard;

			UIGrid = new Grid();

			dataArray[0] = new FastFixedSizeLabel[Constants.NumberOfFrets];
			dataArray[1] = new FastFixedSizeLabel[Constants.NumberOfFrets];
			dataArray[2] = new FastFixedSizeLabel[Constants.NumberOfFrets];
			dataArray[3] = new FastFixedSizeLabel[Constants.NumberOfFrets];
			dataArray[4] = new FastFixedSizeLabel[Constants.NumberOfFrets];
			dataArray[5] = new FastFixedSizeLabel[Constants.NumberOfFrets];
			dataArray[6] = new FastFixedSizeLabel[Constants.NumberOfFrets];

			FillDataArray();
			FillFretBoardGrid();
		}
		private void FillDataArray()
		{
			List<GuitarString> guitarStrings = EnumCollectionCreator<GuitarString>.GetEnumCollection().ToList();

			for (int stringIndex = 0; stringIndex < guitarStrings.Count(); stringIndex++)
			{
				GuitarString guitarString = guitarStrings[stringIndex];

				for (int posIndex = 0; posIndex < FretBoardLayoutProvider.FretBoardLayout[guitarString].Count(); posIndex++)
				{
					FretBoardPosition fretBoardPosition = FretBoardLayoutProvider.FretBoardLayout[guitarString][posIndex];
					FastFixedSizeLabel label = GetLabel(fretBoardPosition, posIndex);

					dataArray[stringIndex][posIndex] = label;
				}
			}

			//add fret numbers
			int indexLastRow = dataArray.GetLength(0) - 1;
			for (int fretIndex = 0; fretIndex <= Constants.NumberOfFrets - 1; fretIndex++)
			{
				FastFixedSizeLabel label = new FastFixedSizeLabel(5, 5)
				{
					Text = fretIndex.ToString(),
				};

				dataArray[indexLastRow][fretIndex] = label;
			}
		}

		private void FillFretBoardGrid()
		{
			UIGrid.Children.Add(GetImage());

			UIGrid.HorizontalOptions = LayoutOptions.Center;
			UIGrid.VerticalOptions = LayoutOptions.Center;

			//setup grid
			for (int stringNumber = 0; stringNumber < Constants.NumberOfStrings + 1; stringNumber++)
			{
				UIGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.5, GridUnitType.Star) });
			}

			for (int fretnumber = 0; fretnumber <= Constants.NumberOfFrets - 1; fretnumber++)
			{
				UIGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Star });
			}

			//add capo
			Frame capo = new Frame()
			{
				Padding = 0,
				BackgroundColor = Color.Transparent,
				CornerRadius = 25
			};
			UIGrid.Children.Add(capo, 0, 0);
			Grid.SetRowSpan(capo, 6);

			//add labels for fretboard position keys
			for (int rowIndex = 0; rowIndex < dataArray.GetLength(0); rowIndex++)
			{
				for (int columnIndex = 0; columnIndex < dataArray[rowIndex].Length; columnIndex++)
				{
					FastFixedSizeLabel label = dataArray[rowIndex][columnIndex];
					UIGrid.Children.Add(label, columnIndex, rowIndex);
				}
			}
		}

		public void UpdateGrid(GuitarString guitarString)
		{
			FillDataArray();

			List<GuitarString> guitarStrings = EnumCollectionCreator<GuitarString>.GetEnumCollection().ToList();
			int stringIndex = guitarStrings.IndexOf(guitarString);

			for (int posIndex = 0; posIndex < dataArray[stringIndex].Length; posIndex++)
			{
				FretBoardPosition fretBoardPosition = FretBoardLayoutProvider.FretBoardLayout[guitarString][posIndex];
				FastFixedSizeLabel label = UIGrid.Children
					.Where(c => c.GetType() == typeof(FastFixedSizeLabel))
					.Cast<FastFixedSizeLabel>()
					.First(e => Grid.GetRow(e) == stringIndex && Grid.GetColumn(e) == posIndex && Grid.GetRowSpan(e) == 1);

				label.Text = GetFretBoardPositionKeyText(fretBoardPosition, posIndex);
				label.BackgroundColor = posIndex < FretBoardLayoutProvider.CapoPosition ? Color.Transparent : GetNoteColor(fretBoardPosition);
				label.BorderColor = !fretBoardPosition.IsScaleNote || posIndex < FretBoardLayoutProvider.CapoPosition ? Color.Transparent : Color.Gray;

				if (fretBoardPosition.IsScaleNote && posIndex >= FretBoardLayoutProvider.CapoPosition)
				{
					string file = fretBoardPosition.PianoOctave + "_" + fretBoardPosition.Key.ToString().ToLower() + ".mid";
					label.OnTouchAction = () => KeyPlayer.KeyPlayer.PlaySound(file);
				}
				else
				{
					label.OnTouchAction = () => { };
				}
			}
		}

		public void UpdateCapo()
		{
			Frame capo = UIGrid.Children
					.Where(c => c.GetType() == typeof(Frame))
					.Cast<Frame>()
					.First(e => Grid.GetRow(e) == 0 && Grid.GetRowSpan(e) == Constants.NumberOfStrings);

			capo.BackgroundColor = FretBoardLayoutProvider.CapoPosition > 0 ? Color.FromHex("C6B598") : Color.Transparent;
			Grid.SetColumn(capo, FretBoardLayoutProvider.CapoPosition);
		}

		private FastFixedSizeLabel GetLabel(FretBoardPosition fretBoardPosition, int posIndex)
		{
			return new FastFixedSizeLabel(25, 40)
			{
				Text = GetFretBoardPositionKeyText(fretBoardPosition, posIndex),
				CornerRadius = 25,
				Margin = 2.5,
				BorderWidth = 2,
			};
		}
		private static Image GetImage()
		{
			Image image = new Image();
			image.Source = "FretBoardColorless23.jpg";
			Grid.SetColumn(image, 0);
			Grid.SetColumnSpan(image, Constants.NumberOfFrets);
			Grid.SetRow(image, 0);
			Grid.SetRowSpan(image, Constants.NumberOfStrings);
			image.Aspect = Aspect.Fill;
			image.VerticalOptions = LayoutOptions.Fill;
			image.HorizontalOptions = LayoutOptions.Center;
			image.ScaleX = 1.013;
			image.ScaleY = 0.95;
			return image;
		}
		private Color GetNoteColor(FretBoardPosition fretBoardPosition)
		{
			return fretBoardPosition.IsRootNote ? Color.FromHex("FDCF76") : fretBoardPosition.IsScaleNote ? Color.FromHex("DD4124") : Color.Transparent;
		}

		private string GetFretBoardPositionKeyText(FretBoardPosition fretBoardPosition, int posIndex)
		{
			switch (Settings.KeyDisplayingSettings)
			{
				case KeyDisplayingSettings.ALL:
					switch (Settings.SemiStepSettings)
					{
						case SemiStepSettings.SHARP:
						case SemiStepSettings.FLAT:
							return fretBoardPosition.Key.GetKeyDiscription();

						case SemiStepSettings.INTERVAL:
							return fretBoardPosition.Interval.ToString();

						default:
							throw new ArgumentException($"unhandled enum: {nameof(SemiStepSettings)}");
					}

				case KeyDisplayingSettings.SCALE:
					switch (Settings.SemiStepSettings)
					{
						case SemiStepSettings.SHARP:
						case SemiStepSettings.FLAT:
							return fretBoardPosition.IsScaleNote && !(posIndex < FretBoardLayoutProvider.CapoPosition) ? fretBoardPosition.Key.GetKeyDiscription() : string.Empty;

						case SemiStepSettings.INTERVAL:
							return fretBoardPosition.IsScaleNote && !(posIndex < FretBoardLayoutProvider.CapoPosition) ? fretBoardPosition.Interval.ToString() : string.Empty;

						default:
							throw new ArgumentException($"unhandled enum: {nameof(SemiStepSettings)}");
					}

				case KeyDisplayingSettings.NONE:
					return string.Empty;

				default:
					throw new ArgumentException($"unhandled enum: {nameof(KeyDisplayingSettings)}");
			}
		}
	}
}
