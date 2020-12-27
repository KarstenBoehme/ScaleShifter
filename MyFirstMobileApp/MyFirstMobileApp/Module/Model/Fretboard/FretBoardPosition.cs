using MyFirstMobileApp.Module.Properties;

namespace MyFirstMobileApp.Module.Fretboard
{
	public class FretBoardPosition
    {
        public Key Key { get; set; }
        public int PianoOctave { get; set; }
        public Interval Interval { get; set; }
        public bool IsScaleNote { get; set; }
        public bool IsRootNote { get; set; }
        public FretBoardPosition(Key key)
        {
            this.Key = key;
        }
	}
}
