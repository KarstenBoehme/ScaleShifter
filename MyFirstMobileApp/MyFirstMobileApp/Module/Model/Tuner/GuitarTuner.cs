using Android;
using Android.App;
using Android.Content.PM;
using Android.Media;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace MyFirstMobileApp.Module.Tuner
{
	public class GuitarTuner
	{
		private readonly double MinFreq = 40;		//E1
		private readonly double MaxFreq = 1760;		//A6
		private readonly int SampleRate = 44100;

		public AudioRecord audRecorder;
		public BehaviorSubject<double> Frequency;

		public GuitarTuner()
		{
			Frequency = new BehaviorSubject<double>(0);
		}

		public void RecordAudio()
		{
			byte[] audioBuffer = new byte[100000];
			audRecorder = new AudioRecord(AudioSource.Mic, SampleRate, ChannelIn.Mono, Android.Media.Encoding.Pcm16bit, audioBuffer.Length);

			audRecorder.StartRecording();

			while (true)
			{
				try
				{
					audRecorder.Read(audioBuffer, 0, audioBuffer.Length);
					double[] _audioBuffer = BytesToDouble(audioBuffer);

					bool isWithinTh = IsWithinThreshold(_audioBuffer, 1000, 65000);

					if(isWithinTh)
					{
						double freq = FrequencyUtils.FindFundamentalFrequency(BytesToDouble(audioBuffer), SampleRate, MinFreq, MaxFreq);
						Frequency.OnNext(Math.Round(freq, 2));
					}
					else
					{
						Frequency.OnNext(0);
					}
				}
				catch
				{
					//TODO implement logger
				}
			}
		}
		private static double[] BytesToDouble(byte[] bytes)
		{
			double[] floats = new double[bytes.Length / 2];
			for (int i = 0; i < bytes.Length; i += 2)
			{
				floats[i / 2] = bytes[i] | (bytes[i + 1] << 8);
			}
			return floats;
		}

		private bool IsWithinThreshold(double[] buffer, int minThreshold, int maxThreshold)
		{
			for (int index = 0; index < buffer.Length; index++)
			{
				if (Math.Abs(buffer[index]) > minThreshold && Math.Abs(buffer[index]) < maxThreshold)
				{
					return true;
				}
			}
			return false; //not found
		}
	}
}
