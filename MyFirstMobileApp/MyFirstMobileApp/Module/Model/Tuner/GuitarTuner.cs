using Android.Media;
using System;
using System.Reactive.Subjects;

namespace MyFirstMobileApp.Module.Tuner
{
	public class GuitarTuner
	{
		private AudioRecord audRecorder;
		private readonly double MinFreq = 40;		//E1
		private readonly double MaxFreq = 1760;		//A6
		private readonly int SampleRate = 44100;
		private readonly double Threshold = 0.009;
		private short[] audioBuffer = new short[10000];

		public BehaviorSubject<double> Frequency { get; set; }
		public BehaviorSubject<double> Buffer { get; set; }

		public GuitarTuner()
		{
			Frequency = new BehaviorSubject<double>(0);
			Buffer = new BehaviorSubject<double>(0);
		}

		public void RecordAudio()
		{
			audRecorder = new AudioRecord(AudioSource.Mic, SampleRate, ChannelIn.Mono, Encoding.Pcm16bit, audioBuffer.Length);
			audRecorder.StartRecording();

			while (true)
			{
				try
				{
					audRecorder.Read(audioBuffer, 0, audioBuffer.Length);
					double[] _audioBuffer = ShortToDouble(audioBuffer);
					bool isSignal = IsSignal(_audioBuffer);

					if(isSignal)
					{
						double freq = FrequencyUtils.FindFundamentalFrequency(_audioBuffer, SampleRate, MinFreq, MaxFreq);
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

		public void StopRecording()
		{
			if (audRecorder != null)
				audRecorder.Release();
		}
		private double[] ShortToDouble(short[] shorts)
		{
			double[] _audioBuffer = new double[shorts.Length];
			for (int i = 0; i < _audioBuffer.Length; i++)
			{
				_audioBuffer[i] = shorts[i] / 32768.0;
			}
			return _audioBuffer;
		}

		private bool IsSignal(double[] buffer)
		{
			for (int index = 0; index < buffer.Length; index++)
			{
				Buffer.OnNext(buffer[index]);
				if (Math.Abs(buffer[index]) > Threshold)
				{
					return true;
				}
			}
			return false; //not signal
		}
	}
}
