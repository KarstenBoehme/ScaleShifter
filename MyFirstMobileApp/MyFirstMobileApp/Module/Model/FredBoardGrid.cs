using Android.Widget;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xamarin.Forms;

namespace MyFirstMobileApp
{
	public class FredBoardGrid
	{
		private Frame[][] dataArray = new Frame[7][];
		public Grid UIGrid { get; private set; }

		public FredBoardGrid(Dictionary<GuitarString, List<FretBoardPosition>> fretBoardLayout)
		{
			UIGrid = new Grid();

			dataArray[0] = new Frame[13];
			dataArray[1] = new Frame[13];
			dataArray[2] = new Frame[13];
			dataArray[3] = new Frame[13];
			dataArray[4] = new Frame[13];
			dataArray[5] = new Frame[13];
			dataArray[6] = new Frame[13];

			FillDataArray(fretBoardLayout);
			FillFretBoardGrid();
		}
		private void FillDataArray(Dictionary<GuitarString, List<FretBoardPosition>> fretBoardLayout)
		{
			List<GuitarString> guitarStrings = EnumCollectionCreator<GuitarString>.GetEnumCollection().ToList();

			for (int stringIndex = 0; stringIndex < guitarStrings.Count(); stringIndex++)
			{
				GuitarString guitarString = guitarStrings[stringIndex];

				for (int posIndex = 0; posIndex < fretBoardLayout[guitarString].Count() + 1; posIndex++)
				{
					FretBoardPosition fretBoardPosition = posIndex == 12 ?
						fretBoardLayout[guitarString][0] :
						fretBoardLayout[guitarString][posIndex];

					Label label = GetLabel(fretBoardPosition);
					Frame frame = GetFrame(fretBoardPosition, label);
					dataArray[stringIndex][posIndex] = frame;
				}
			}

			//add fret numbers
			int indexLastRow = dataArray.GetLength(0) - 1;
			for (int fretIndex = 0; fretIndex <= Constants.NumberOfNotes; fretIndex++)
			{
				Label label = new Label()
				{
					Text = (fretIndex).ToString(),
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
				};
				Frame frame = new Frame()
				{
					Content = label,
					Padding = 0,
					BackgroundColor = Color.Transparent,
				};

				dataArray[indexLastRow][fretIndex] = frame;
			}
		}

		private void FillFretBoardGrid()
		{	
			UIGrid.Children.Add(GetImage());

			//setup grid
			for (int stringNumber = 0; stringNumber < Constants.NumberOfStrings + 1; stringNumber++)
			{
				UIGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(0.5, GridUnitType.Star) });
			}

			for (int fretnumber = 0; fretnumber < Constants.NumberOfNotes + 1; fretnumber++)
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

			//add frames for keys
			for (int rowIndex = 0; rowIndex < dataArray.GetLength(0); rowIndex++)
			{
				for (int columnIndex = 0; columnIndex < dataArray[rowIndex].Length; columnIndex++)
				{
					Frame frame = dataArray[rowIndex][columnIndex];
					UIGrid.Children.Add(frame, columnIndex, rowIndex);
				}
			}
		}

		public void UpdateGrid(Dictionary<GuitarString, List<FretBoardPosition>> fretBoardLayout, GuitarString guitarString)
		{
			FillDataArray(fretBoardLayout);

			List<GuitarString> guitarStrings = EnumCollectionCreator<GuitarString>.GetEnumCollection().ToList();
			int stringIndex = guitarStrings.IndexOf(guitarString);

			for (int posIndex = 0; posIndex < dataArray[stringIndex].Length; posIndex++)
			{
				FretBoardPosition fretBoardPosition = posIndex == 12 ?
						fretBoardLayout[guitarString][0] :
						fretBoardLayout[guitarString][posIndex];

				Frame frame = UIGrid.Children
					.Where(c => c.GetType() == typeof(Frame))
					.Cast<Frame>()
					.First(e => Grid.GetRow(e) == stringIndex && Grid.GetColumn(e) == posIndex && Grid.GetRowSpan(e) == 1);
				Label label = ((Label)frame.Content);

				label.Text = GetFretBoardPositionKeyText(fretBoardPosition);
				frame.BackgroundColor = GetNoteColor(fretBoardPosition);
			}
		}

		public void UpdateCapoSetup(int capoPosition)
		{
			Frame capo = UIGrid.Children
					.Where(c => c.GetType() == typeof(Frame))
					.Cast<Frame>()
					.First(e => Grid.GetRow(e) == 0 && Grid.GetColumn(e) == 0 && Grid.GetRowSpan(e) == Constants.NumberOfStrings);
			
			capo.BackgroundColor = capoPosition > 0 ? Color.FromHex("C6B598") : Color.Transparent;

			for (int posIndex = 0; posIndex < Constants.NumberOfNotes + 1; posIndex++)
			{
				Frame frame = UIGrid.Children
					.Where(c => c.GetType() == typeof(Frame))
					.Cast<Frame>()
					.First(e => Grid.GetRow(e) == 6 && Grid.GetColumn(e) == posIndex);
				Label label = ((Label)frame.Content);

				label.Text = (posIndex + capoPosition).ToString();
			}
		}

		private Frame GetFrame(FretBoardPosition fretBoardPosition, Label label)
		{
			return new Frame
			{
				CornerRadius = 15,
				HeightRequest = 30,
				WidthRequest = 30,
				Padding = 0,
				BackgroundColor = GetNoteColor(fretBoardPosition),
				Content = label
			};
		}
		private Label GetLabel(FretBoardPosition fretBoardPosition)
		{
			return new Label()
			{
				Text = GetFretBoardPositionKeyText(fretBoardPosition),
				TextColor = Color.Black,
				Padding = 0,
				FontAttributes = FontAttributes.Bold,
				FontSize = 11,
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center
			};
		}
		private static Image GetImage()
		{
			Image image = new Image();
			image.Source = "FretBoardColorless.jpg";
			Grid.SetColumn(image, 0);
			Grid.SetColumnSpan(image, Constants.NumberOfNotes + 1);
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

		private string GetFretBoardPositionKeyText(FretBoardPosition fretBoardPosition)
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
							return fretBoardPosition.IsScaleNote ? fretBoardPosition.Key.GetKeyDiscription() : string.Empty;

						case SemiStepSettings.INTERVAL:
							return fretBoardPosition.IsScaleNote ? fretBoardPosition.Interval.ToString() : string.Empty;
						
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
