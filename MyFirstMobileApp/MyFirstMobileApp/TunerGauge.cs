using MyFirstMobileApp.Module;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

namespace MyFirstMobileApp
{
	public class TunerGauge : SKCanvasView
	{
		public static readonly BindableProperty FrequencyProperty =
			BindableProperty.Create("Frequency", typeof(double), typeof(TunerGauge), 0.0);

		public double Frequency
		{
			get { return (double)GetValue(FrequencyProperty); }
			set { SetValue(FrequencyProperty, value); }
		}

		public static readonly BindableProperty SetFrequencyProperty =
			BindableProperty.Create("SetFrequency", typeof(double), typeof(TunerGauge), 0.0);

		public double SetFrequency
		{
			get { return (double)GetValue(SetFrequencyProperty); }
			set { SetValue(SetFrequencyProperty, value); }
		}

		private SKPaint Minus40CtPaint { get; set; }
		private SKPaint Minus30CtPaint { get; set; }
		private SKPaint Minus20CtPaint { get; set; }
		private SKPaint Minus10CtPaint { get; set; }
		private SKPaint zeroCtPaint { get; set; }
		private SKPaint Plus10CtPaint { get; set; }
		private SKPaint Plus20CtPaint { get; set; }
		private SKPaint Plus30CtPaint { get; set; }
		private SKPaint Plus40CtPaint { get; set; }

		private static readonly int elementWidth = 50;
		private static readonly int radius = elementWidth / 2;
		private static readonly int strokeWidth = 10;
		private static readonly double gapfactor = 0.2;

		public TunerGauge()
		{
			Minus40CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner40ct),
				StrokeWidth = strokeWidth,
			};

			Minus30CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner20ct),
				StrokeWidth = strokeWidth,
			};

			Minus20CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner20ct),
				StrokeWidth = strokeWidth,
			};

			Minus10CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner10ct),
				StrokeWidth = strokeWidth,
			};

			zeroCtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.TunerZero),
				StrokeWidth = strokeWidth,
			};

			Plus10CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner10ct),
				StrokeWidth = strokeWidth,
			};

			Plus20CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner20ct),
				StrokeWidth = strokeWidth,
			};

			Plus30CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner30ct),
				StrokeWidth = strokeWidth,
			};

			Plus40CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner40ct),
				StrokeWidth = strokeWidth,
			};
		}
		protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
		{
			base.OnPaintSurface(e);

			var canvas = e.Surface.Canvas;
			canvas.Clear();

			int width = e.Info.Width;
			int height = e.Info.Height;

			canvas.Save();
			canvas.Translate(width / 2, (float)(height * 0.9));
			canvas.Scale(-1, -1);

			SKRect zeroCtRect = new SKRect();
			zeroCtRect.Size = new SKSize(elementWidth, elementWidth + 110);
			zeroCtRect.Location = new SKPoint((float)(-elementWidth / 2), 0);

			SKRect minus10ctRect = new SKRect();
			minus10ctRect.Size = new SKSize(elementWidth, elementWidth + 50);
			minus10ctRect.Location = new SKPoint((float)(zeroCtRect.Location.X - elementWidth * (1.4 + gapfactor)), 0);

			SKRect minus20ctRect = new SKRect();
			minus20ctRect.Size = new SKSize(elementWidth, elementWidth + 25);
			minus20ctRect.Location = new SKPoint((float)(zeroCtRect.Location.X - elementWidth * (2.9 + gapfactor)), 0);

			SKRect minus30ctRect = new SKRect();
			minus30ctRect.Size = new SKSize(elementWidth, elementWidth + 5);
			minus30ctRect.Location = new SKPoint((float)(zeroCtRect.Location.X - elementWidth * (4.4 + gapfactor)), 0);

			SKRect minus40ctRect = new SKRect();
			minus40ctRect.Size = new SKSize(elementWidth, elementWidth);
			minus40ctRect.Location = new SKPoint((float)(zeroCtRect.Location.X - elementWidth * (5.8 + gapfactor)), 0);

			SKRect plus10ctRect = new SKRect();
			plus10ctRect.Size = minus10ctRect.Size;
			plus10ctRect.Location = new SKPoint((float)(elementWidth * (0.9 + gapfactor)), 0);

			SKRect plus20ctRect = new SKRect();
			plus20ctRect.Size = minus20ctRect.Size;
			plus20ctRect.Location = new SKPoint((float)(elementWidth * (2.4 + gapfactor)), 0);

			SKRect plus30ctRect = new SKRect();
			plus30ctRect.Size = minus30ctRect.Size;
			plus30ctRect.Location = new SKPoint((float)(elementWidth * (3.9 + gapfactor)), 0);

			SKRect plus40ctRect = new SKRect();
			plus40ctRect.Size = minus40ctRect.Size;
			plus40ctRect.Location = new SKPoint((float)(elementWidth * (5.3 + gapfactor)), 0);

			canvas.DrawRoundRect(new SKRoundRect(minus40ctRect, radius), Minus40CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus30ctRect, radius), Minus30CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus20ctRect, radius), Minus20CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus10ctRect, radius), Minus10CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(zeroCtRect, radius), zeroCtPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus10ctRect, radius), Plus10CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus20ctRect, radius), Plus20CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus30ctRect, radius), Plus30CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus40ctRect, radius), Plus40CtPaint);
		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == FrequencyProperty.PropertyName
				|| propertyName == SetFrequencyProperty.PropertyName
				|| propertyName == WidthProperty.PropertyName
				|| propertyName == HeightProperty.PropertyName)
			{
				ChangeStyle();
				InvalidateSurface();
			}
		}

		private void ChangeStyle()
		{
			double ct = 1200 * Math.Log((SetFrequency / Frequency), 2);

			if(ct == 0)
			{
				Minus40CtPaint.Style = SKPaintStyle.Stroke;
				Minus30CtPaint.Style = SKPaintStyle.Stroke;
				Minus20CtPaint.Style = SKPaintStyle.Stroke;
				Minus10CtPaint.Style = SKPaintStyle.Stroke;
				zeroCtPaint.Style = SKPaintStyle.Stroke;
				Plus10CtPaint.Style = SKPaintStyle.Stroke;
				Plus20CtPaint.Style = SKPaintStyle.Stroke;
				Plus30CtPaint.Style = SKPaintStyle.Stroke;
				Plus40CtPaint.Style = SKPaintStyle.Stroke;
			}
			else
			{
				Minus40CtPaint.Style = ct > -50 && ct < -40 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
				Minus30CtPaint.Style = ct > -40 && ct < -30 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
				Minus20CtPaint.Style = ct > -30 && ct < -20 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
				Minus10CtPaint.Style = ct > -20 && ct < -10 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
				zeroCtPaint.Style = Math.Abs(ct) <= 10 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
				Plus10CtPaint.Style = ct > 10 && ct < 20 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
				Plus20CtPaint.Style = ct > 20 && ct < 30 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
				Plus30CtPaint.Style = ct > 30 && ct < 40 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
				Plus40CtPaint.Style = ct > 40 && ct < 50 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			}
		}
	}
}
