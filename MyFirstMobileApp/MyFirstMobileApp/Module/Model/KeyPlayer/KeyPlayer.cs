using Android.App;
using Android.Content.Res;
using Android.Media;
using MyFirstMobileApp.Module.Properties;

namespace MyFirstMobileApp.Module.KeyPlayer
{
	public static class KeyPlayer
	{
        public static void PlaySound(string file)
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
    }
}
