using System;
using System.Collections.Generic;
using System.Text;

namespace MyFirstMobileApp.Module.Fretboard
{
	public interface IFretBoardLayoutProvider
	{
		int CapoPosition { get; }
		Dictionary<GuitarString, List<FretBoardPosition>> FretBoardLayout { get; }
	}
}
