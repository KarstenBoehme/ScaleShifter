using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace MyFirstMobileApp
{
    public class FretBoardPosition
    {
        public Key Key { get; set; }
        public bool IsScaleNote { get; set; }
        public bool IsRootNote { get; set; }
        public FretBoardPosition(Key key)
        {
            this.Key = key;
        }
    }
}
