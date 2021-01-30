using MyFirstMobileApp.Module;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using Xamarin.Forms;

namespace MyFirstMobileApp
{
	public class TunerGauge : SKCanvasView
	{
		private static readonly int elementWidth = 45;
		private static readonly int radius = elementWidth / 2 - 1;
		private static readonly int strokeWidth = 7;
		private static readonly int borderStrokeWidth = 2;
		private static readonly int gapfactor = 25;

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
		private SKPaint Minus5CtPaint { get; set; }
		private SKPaint zeroCtPaint { get; set; }
		private SKPaint Plus5CtPaint { get; set; }
		private SKPaint Plus10CtPaint { get; set; }
		private SKPaint Plus20CtPaint { get; set; }
		private SKPaint Plus30CtPaint { get; set; }
		private SKPaint Plus40CtPaint { get; set; }
		private SKPaint BorderPaint { get; set; }

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
				Color = SKColor.Parse(ColorSchemes.Tuner30ct),
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

			Minus5CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner5ct),
				StrokeWidth = strokeWidth,
			};

			zeroCtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.TunerZero),
				StrokeWidth = strokeWidth,
			};

			Plus5CtPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.Tuner5ct),
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

			BorderPaint = new SKPaint()
			{
				Style = SKPaintStyle.Stroke,
				Color = SKColor.Parse(ColorSchemes.BorderNote),
				StrokeWidth = borderStrokeWidth,
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

			SKRect minus5ctRect = new SKRect();
			minus5ctRect.Size = new SKSize(elementWidth, elementWidth + 75);
			minus5ctRect.Location = new SKPoint((float)(zeroCtRect.Location.X - elementWidth - gapfactor), 0);

			SKRect minus10ctRect = new SKRect();
			minus10ctRect.Size = new SKSize(elementWidth, elementWidth + 50);
			minus10ctRect.Location = new SKPoint((float)(minus5ctRect.Location.X - elementWidth - gapfactor), 0);

			SKRect minus20ctRect = new SKRect();
			minus20ctRect.Size = new SKSize(elementWidth, elementWidth + 25);
			minus20ctRect.Location = new SKPoint((float)(minus10ctRect.Location.X - elementWidth - gapfactor), 0);

			SKRect minus30ctRect = new SKRect();
			minus30ctRect.Size = new SKSize(elementWidth, elementWidth + 5);
			minus30ctRect.Location = new SKPoint((float)(minus20ctRect.Location.X - elementWidth - gapfactor), 0);

			SKRect minus40ctRect = new SKRect();
			minus40ctRect.Size = new SKSize(elementWidth, elementWidth);
			minus40ctRect.Location = new SKPoint((float)(minus30ctRect.Location.X - elementWidth - gapfactor), 0);


			SKRect plus5ctRect = new SKRect();
			plus5ctRect.Size = minus5ctRect.Size;
			plus5ctRect.Location = new SKPoint((float)(zeroCtRect.Location.X + elementWidth + gapfactor), 0);

			SKRect plus10ctRect = new SKRect();
			plus10ctRect.Size = minus10ctRect.Size;
			plus10ctRect.Location = new SKPoint((float)(plus5ctRect.Location.X + elementWidth + gapfactor), 0);

			SKRect plus20ctRect = new SKRect();
			plus20ctRect.Size = minus20ctRect.Size;
			plus20ctRect.Location = new SKPoint((float)(plus10ctRect.Location.X + elementWidth + gapfactor), 0);

			SKRect plus30ctRect = new SKRect();
			plus30ctRect.Size = minus30ctRect.Size;
			plus30ctRect.Location = new SKPoint((float)(plus20ctRect.Location.X + elementWidth + gapfactor), 0);

			SKRect plus40ctRect = new SKRect();
			plus40ctRect.Size = minus40ctRect.Size;
			plus40ctRect.Location = new SKPoint((float)(plus30ctRect.Location.X + elementWidth + gapfactor), 0);


			canvas.DrawRoundRect(new SKRoundRect(minus40ctRect, radius), Minus40CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus30ctRect, radius), Minus30CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus20ctRect, radius), Minus20CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus10ctRect, radius), Minus10CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus5ctRect, radius), Minus5CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(zeroCtRect, radius), zeroCtPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus5ctRect, radius), Plus5CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus10ctRect, radius), Plus10CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus20ctRect, radius), Plus20CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus30ctRect, radius), Plus30CtPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus40ctRect, radius), Plus40CtPaint);


			SKRect zeroCtBorderRect = new SKRect();
			zeroCtBorderRect.Size = new SKSize(zeroCtRect.Width + strokeWidth, zeroCtRect.Height + strokeWidth);
			zeroCtBorderRect.Location = new SKPoint((float)(zeroCtRect.Location.X - 0.5 * strokeWidth), (float)(zeroCtRect.Location.Y - 0.5 * strokeWidth));

			SKRect minus5CtBorderRect = new SKRect();
			minus5CtBorderRect.Size = new SKSize(minus5ctRect.Width + strokeWidth, minus5ctRect.Height + strokeWidth);
			minus5CtBorderRect.Location = new SKPoint((float)(minus5ctRect.Location.X - 0.5 * strokeWidth), (float)(minus5ctRect.Location.Y - 0.5 * strokeWidth));

			SKRect minus10CtBorderRect = new SKRect();
			minus10CtBorderRect.Size = new SKSize(minus10ctRect.Width + strokeWidth, minus10ctRect.Height + strokeWidth);
			minus10CtBorderRect.Location = new SKPoint((float)(minus10ctRect.Location.X - 0.5 * strokeWidth), (float)(minus10ctRect.Location.Y - 0.5 * strokeWidth));

			SKRect minus20CtBorderRect = new SKRect();
			minus20CtBorderRect.Size = new SKSize(minus20ctRect.Width + strokeWidth, minus20ctRect.Height + strokeWidth);
			minus20CtBorderRect.Location = new SKPoint((float)(minus20ctRect.Location.X - 0.5 * strokeWidth), (float)(minus20ctRect.Location.Y - 0.5 * strokeWidth));

			SKRect minus30CtBorderRect = new SKRect();
			minus30CtBorderRect.Size = new SKSize(minus30ctRect.Width + strokeWidth, minus30ctRect.Height + strokeWidth);
			minus30CtBorderRect.Location = new SKPoint((float)(minus30ctRect.Location.X - 0.5 * strokeWidth), (float)(minus30ctRect.Location.Y - 0.5 * strokeWidth));

			SKRect minus40CtBorderRect = new SKRect();
			minus40CtBorderRect.Size = new SKSize(minus40ctRect.Width + strokeWidth, minus40ctRect.Height + strokeWidth);
			minus40CtBorderRect.Location = new SKPoint((float)(minus40ctRect.Location.X - 0.5 * strokeWidth), (float)(minus40ctRect.Location.Y - 0.5 * strokeWidth));


			SKRect plus5CtBorderRect = new SKRect();
			plus5CtBorderRect.Size = new SKSize(plus5ctRect.Width + strokeWidth, plus5ctRect.Height + strokeWidth);
			plus5CtBorderRect.Location = new SKPoint((float)(plus5ctRect.Location.X - 0.5 * strokeWidth), (float)(plus5ctRect.Location.Y - 0.5 * strokeWidth));

			SKRect plus10CtBorderRect = new SKRect();
			plus10CtBorderRect.Size = new SKSize(plus10ctRect.Width + strokeWidth, plus10ctRect.Height + strokeWidth);
			plus10CtBorderRect.Location = new SKPoint((float)(plus10ctRect.Location.X - 0.5 * strokeWidth), (float)(plus10ctRect.Location.Y - 0.5 * strokeWidth));

			SKRect plus20CtBorderRect = new SKRect();
			plus20CtBorderRect.Size = new SKSize(plus20ctRect.Width + strokeWidth, plus20ctRect.Height + strokeWidth);
			plus20CtBorderRect.Location = new SKPoint((float)(plus20ctRect.Location.X - 0.5 * strokeWidth), (float)(plus20ctRect.Location.Y - 0.5 * strokeWidth));

			SKRect plus30CtBorderRect = new SKRect();
			plus30CtBorderRect.Size = new SKSize(plus30ctRect.Width + strokeWidth, plus30ctRect.Height + strokeWidth);
			plus30CtBorderRect.Location = new SKPoint((float)(plus30ctRect.Location.X - 0.5 * strokeWidth), (float)(plus30ctRect.Location.Y - 0.5 * strokeWidth));

			SKRect plus40CtBorderRect = new SKRect();
			plus40CtBorderRect.Size = new SKSize(plus40ctRect.Width + strokeWidth, plus40ctRect.Height + strokeWidth);
			plus40CtBorderRect.Location = new SKPoint((float)(plus40ctRect.Location.X - 0.5 * strokeWidth), (float)(plus40ctRect.Location.Y - 0.5 * strokeWidth));


			canvas.DrawRoundRect(new SKRoundRect(minus40CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus30CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus20CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus10CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(minus5CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(zeroCtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus5CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus10CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus20CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus30CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
			canvas.DrawRoundRect(new SKRoundRect(plus40CtBorderRect, (float)(radius + 0.5 * strokeWidth)), BorderPaint);
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

			Minus40CtPaint.Style = ct > -50 && ct < -40 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			Minus30CtPaint.Style = ct > -40 && ct < -30 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			Minus20CtPaint.Style = ct > -30 && ct < -20 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			Minus10CtPaint.Style = ct > -20 && ct < -10 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			Minus5CtPaint.Style = ct > -10 && ct < -5 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			zeroCtPaint.Style = Math.Abs(ct) <= 5 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			Plus5CtPaint.Style = ct > 5 && ct < 10 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			Plus10CtPaint.Style = ct > 10 && ct < 20 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			Plus20CtPaint.Style = ct > 20 && ct < 30 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			Plus30CtPaint.Style = ct > 30 && ct < 40 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
			Plus40CtPaint.Style = ct > 40 && ct < 50 ? SKPaintStyle.StrokeAndFill : SKPaintStyle.Stroke;
		}
	}
}
