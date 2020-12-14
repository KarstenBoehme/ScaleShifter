﻿using System;
using Xamarin.Forms;

namespace MyFirstMobileApp
{
	public class FastFixedSizeLabel : Label
	{
        public static readonly BindableProperty TextProperty = 
            BindableProperty.Create("Text", typeof(string), typeof(FastFixedSizeLabel), "");

        public static readonly BindableProperty TextColorProperty = 
            BindableProperty.Create("TextColor", typeof(Color), typeof(FastFixedSizeLabel), Color.Black);

        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create("BorderColor", typeof(Color), typeof(FastFixedSizeLabel), Color.Transparent);

        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create("BorderWidth", typeof(int), typeof(FastFixedSizeLabel), 0);

        public static readonly BindableProperty CornerRadiusProperty =
             BindableProperty.Create("CornerRadius", typeof(float), typeof(FastFixedSizeLabel));


        public readonly double FixedWidth;

        public readonly double FixedHeight;


        public FastFixedSizeLabel(double width, double height)
        {
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
                SetValue(TextColorProperty, value);
                OnPropertyChanged(nameof(TextColor));
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
                SetValue(TextProperty, value);
                OnPropertyChanged(nameof(Text));
            }
        }

        public Color BorderColor
        {
            get
            {
                return (Color)GetValue(BorderColorProperty);
            }
            set
            {
                SetValue(BorderColorProperty, value);
                OnPropertyChanged(nameof(BorderColor));
            }
        }

        public int BorderWidth
        {
            get
            {
                return (int)GetValue(BorderWidthProperty);
            }
            set
            {
                SetValue(BorderWidthProperty, value);
                OnPropertyChanged(nameof(BorderWidth));
            }
        }

        public float CornerRadius
        {
            get
            {
                return (float)GetValue(CornerRadiusProperty);
            }
            set
            {
                SetValue(CornerRadiusProperty, value);
                OnPropertyChanged(nameof(CornerRadius));
            }
        }

        protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
        {
            return new SizeRequest(new Size(FixedWidth, FixedHeight));
        }
    }
}
