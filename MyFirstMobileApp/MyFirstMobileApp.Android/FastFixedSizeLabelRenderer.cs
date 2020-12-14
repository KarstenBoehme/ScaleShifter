using Android.Graphics.Drawables;
using Android.Views;
using Android.Widget;
using MyFirstMobileApp;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(FastFixedSizeLabel), typeof(MyFirstMobileApp.Droid.FastFixedSizeLabelRenderer))]

namespace MyFirstMobileApp.Droid
{
	public class FastFixedSizeLabelRenderer : LabelRenderer
    {
        public TextView TextView;
        public FastFixedSizeLabelRenderer() { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var label = Element as FastFixedSizeLabel;

            TextView = new TextView(Context);
            TextView.Text = label.Text;
            TextView.TextSize = 10;
            TextView.Gravity = GravityFlags.Center;
            //TextView.SetTypeface(Android.Graphics.Typeface.Default, Android.Graphics.TypefaceStyle.Bold);

            SetNativeControl(TextView);
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var label = Element as FastFixedSizeLabel;

            var background = new GradientDrawable();
            background.SetCornerRadius((int)label.CornerRadius);
            background.SetStroke((int)label.BorderWidth, label.BorderColor.ToAndroid());
            background.SetColor(label.BackgroundColor.ToAndroid());

            SetBackgroundDrawable(background);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == FastFixedSizeLabel.TextProperty.PropertyName)
                TextView.Text = (Element as FastFixedSizeLabel).Text;

            if (e.PropertyName == FastFixedSizeLabel.BackgroundColorProperty.PropertyName)
                Control.Invalidate();

            if (e.PropertyName == FastFixedSizeLabel.BorderColorProperty.PropertyName)
                Control.Invalidate();

            if (e.PropertyName == FastFixedSizeLabel.BorderWidthProperty.PropertyName)
                Control.Invalidate();

            if (e.PropertyName == FastFixedSizeLabel.CornerRadiusProperty.PropertyName)
                Control.Invalidate();
        }
    }
}
