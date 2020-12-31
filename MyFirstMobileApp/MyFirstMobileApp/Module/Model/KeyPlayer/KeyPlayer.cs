using Android.App;
using Android.Content.Res;
using Android.Media;
using MyFirstMobileApp.Module.Fretboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFirstMobileApp.Module.KeyPlayer
{
	public static class KeyPlayer
	{
		public static void PlaySingleNote(FretBoardPosition fretBoardPosition)
		{
			string file = GetMidiFile(fretBoardPosition);
			Play(file);
		}

		public static async Task Strum(Dictionary<GuitarString, List<FretBoardPosition>> fretBoardLayout, int capoPos)
		{
			string fileE4 = GetMidiFile(fretBoardLayout[GuitarString.E4][capoPos]);
			string fileB = GetMidiFile(fretBoardLayout[GuitarString.B][capoPos]);
			string fileG = GetMidiFile(fretBoardLayout[GuitarString.G][capoPos]);
			string fileD = GetMidiFile(fretBoardLayout[GuitarString.D][capoPos]);
			string fileA = GetMidiFile(fretBoardLayout[GuitarString.A][capoPos]);
			string fileE = GetMidiFile(fretBoardLayout[GuitarString.E][capoPos]);

			Play(fileE);
			await Task.Delay(Constants.StrummingPauseInMs);
			Play(fileA);
			await Task.Delay(Constants.StrummingPauseInMs);
			Play(fileD);
			await Task.Delay(Constants.StrummingPauseInMs);
			Play(fileG);
			await Task.Delay(Constants.StrummingPauseInMs);
			Play(fileB);
			await Task.Delay(Constants.StrummingPauseInMs);
			Play(fileE4);
		}

		private static void Play(string file)
		{
			try
			{
				AssetFileDescriptor descriptor = Application.Context.Assets.OpenFd(file);
				MediaPlayer mediaPlayer = new MediaPlayer();

				mediaPlayer.SetVolume(50, 50);
				mediaPlayer.SetDataSource(descriptor.FileDescriptor, descriptor.StartOffset, descriptor.Length);
				mediaPlayer.Prepare();
				mediaPlayer.Start();

				mediaPlayer.Completion += delegate
				{
					mediaPlayer.Release();
					mediaPlayer.Dispose();
				};
			}
			catch
			{
				//TODO implement logger
			}
		}
		private static string GetMidiFile(FretBoardPosition fretBoardPosition)
		{
			return fretBoardPosition.PianoOctave + "_" + fretBoardPosition.Key.ToString().ToLower() + ".mid";
		}
	}
}
