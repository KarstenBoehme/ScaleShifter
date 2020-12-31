using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

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
				label.BackgroundColor = GetNoteBackgroundColor(fretBoardPosition, posIndex);
				label.BorderColor = GetNoteBorderColor(fretBoardPosition, posIndex);
				label.BorderWidth = GetNoteBorderWidth();
				label.OnTouchAction = GetOnTouchAction(fretBoardPosition, posIndex);
			}
		}

		public void UpdateCapo()
		{
			Frame capo = UIGrid.Children
					.Where(c => c.GetType() == typeof(Frame))
					.Cast<Frame>()
					.First(e => Grid.GetRow(e) == 0 && Grid.GetRowSpan(e) == Constants.NumberOfStrings);

			capo.BackgroundColor = FretBoardLayoutProvider.CapoPosition > 0 ? 
									Color.FromHex(ColorSchemes.Capo) : 
									Color.Transparent;

			Grid.SetColumn(capo, FretBoardLayoutProvider.CapoPosition);
		}

		public void UpdateFretbordOrientation()
		{
			var labels = UIGrid.Children
					.Where(c => c.GetType() == typeof(FastFixedSizeLabel))
					.Cast<FastFixedSizeLabel>();

			foreach(FastFixedSizeLabel label in labels)
			{
				label.ScaleX = Settings.FretboardOrientationSettings == FretboardOrientationSettings.RIGHT_HAND ? 1 : -1;
			}
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

		private Color GetNoteBackgroundColor(FretBoardPosition fretBoardPosition, int posIndex)
		{
			return	fretBoardPosition.IsRootNote && posIndex >= FretBoardLayoutProvider.CapoPosition ? Color.FromHex(ColorSchemes.RootNote) :
					fretBoardPosition.IsScaleNote && posIndex >= FretBoardLayoutProvider.CapoPosition ? Color.FromHex(ColorSchemes.NonRootNote) :
					Color.Transparent;
		}

		private Color GetNoteBorderColor(FretBoardPosition fretBoardPosition, int posIndex)
		{
			return	Settings.KeyDisplayingSettings == KeyDisplayingSettings.ALL ? Color.FromHex(ColorSchemes.BorderNote) :
					!fretBoardPosition.IsScaleNote || posIndex < FretBoardLayoutProvider.CapoPosition ? Color.Transparent :
					//fretBoardPosition.IsScaleNote && fretBoardPosition.PianoOctave == 1 ? Color.FromHex(ColorSchemes.Border1stOctave) :
					//fretBoardPosition.IsScaleNote && fretBoardPosition.PianoOctave == 2 ? Color.FromHex(ColorSchemes.Border2ndOctave) :
					//fretBoardPosition.IsScaleNote && fretBoardPosition.PianoOctave == 3 ? Color.FromHex(ColorSchemes.Border3rdOctave) :
					//fretBoardPosition.IsScaleNote && fretBoardPosition.PianoOctave == 4 ? Color.FromHex(ColorSchemes.Border4thOctave) :
					//fretBoardPosition.IsScaleNote && fretBoardPosition.PianoOctave == 5 ? Color.FromHex(ColorSchemes.Border5thOctave) :
					//fretBoardPosition.IsScaleNote && fretBoardPosition.PianoOctave == 6 ? Color.FromHex(ColorSchemes.Border6thOctave) :
					Color.FromHex(ColorSchemes.BorderNote);
		}

		private int GetNoteBorderWidth()
		{
			return 2;
		}

		private Action GetOnTouchAction(FretBoardPosition fretBoardPosition, int posIndex)
		{
			if (fretBoardPosition.IsScaleNote && posIndex >= FretBoardLayoutProvider.CapoPosition || 
				Settings.KeyDisplayingSettings == KeyDisplayingSettings.ALL)
			{
				return () => KeyPlayer.KeyPlayer.PlaySingleNote(fretBoardPosition);
			}
			else
			{
				return () => { };
			}
		}
	}
}
