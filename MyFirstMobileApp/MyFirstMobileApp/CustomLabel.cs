using System;
using Xamarin.Forms;

namespace MyFirstMobileApp
{
	public class CustomLabel : Label
	{
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(CustomLabel), "");

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(CustomLabel), Color.Black);

        public readonly double FixedWidth;

        public readonly double FixedHeight;

        public Font Font;

        public LineBreakMode LineBreakMode = LineBreakMode.WordWrap;


        public CustomLabel(string text, double width, double height)
        {
            SetValue(TextProperty, text);
            FixedWidth = width;
            FixedHeight = height;
        }

        public Color TextColor
        {
            get
            {
                return (Color)GetValue(TextColorProperty);
            }
            set
            {
                if (TextColor == value)
                    return;
                SetValue(TextColorProperty, value);
                OnPropertyChanged("TextColor");
            }
        }

        public string Text
        {
            get
            {
                return (string)GetValue(TextProperty);
            }
            set
            {
                if (Text == value)
                    return;
                SetValue(TextProperty, value);
                OnPropertyChanged("Text");
            }
        }

        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(FixedWidth, FixedHeight));
        }
    }
}
