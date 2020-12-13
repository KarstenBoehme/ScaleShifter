using System;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Views;
using Xamarin.Forms;
using MyFirstMobileApp;
using Android.Widget;
using System.ComponentModel;

[assembly: ExportRenderer(typeof(CustomLabel), typeof(MyFirstMobileApp.Droid.CustomLabelRenderer))]

namespace MyFirstMobileApp.Droid
{
	public class CustomLabelRenderer : LabelRenderer
    {
        public CustomLabelRenderer()
        {
        }

        public TextView TextView;

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var label = Element as CustomLabel;
            TextView = new TextView(Context);
            TextView.Text = label.Text;
            TextView.TextSize = 10;
            TextView.Gravity = GravityFlags.Center;
            //TextView.SetTypeface(Android.Graphics.Typeface.Default, Android.Graphics.TypefaceStyle.Bold);
            TextView.SetSingleLine(label.LineBreakMode != LineBreakMode.WordWrap);
            
            if (label.LineBreakMode == LineBreakMode.TailTruncation)
                TextView.Ellipsize = Android.Text.TextUtils.TruncateAt.End;

            SetNativeControl(TextView);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
                TextView.Text = (Element as CustomLabel).Text;

            base.OnElementPropertyChanged(sender, e);
        }
    }
}
