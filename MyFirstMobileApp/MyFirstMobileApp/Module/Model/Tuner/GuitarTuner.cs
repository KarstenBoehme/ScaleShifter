using Android.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;

namespace MyFirstMobileApp.Module.Tuner
{
	public class GuitarTuner
	{
		private AudioRecord audRecorder;
		private readonly double MinFreq = 40;       //E1
		private readonly double MaxFreq = 1760;     //A6
		private readonly int SampleRate = 44100;
		private readonly int MinThreshold = 60;
		private readonly int MaxThreshold = 5600;
		public BehaviorSubject<double> Frequency { get; set; }
		public BehaviorSubject<double> Buffer { get; set; }

		public GuitarTuner()
		{
			Frequency = new BehaviorSubject<double>(0);
			Buffer = new BehaviorSubject<double>(0);
		}

		public void RecordAudio()
		{
			try
			{
				int minBufferSize = AudioRecord.GetMinBufferSize(SampleRate, ChannelIn.Mono, Encoding.Pcm16bit);
				short[] audioBuffer = new short[3 * minBufferSize];

				audRecorder = new AudioRecord(AudioSource.VoiceRecognition, SampleRate, ChannelIn.Mono, Encoding.Pcm16bit, audioBuffer.Length);
				audRecorder.StartRecording();

				while (true)
				{
					audRecorder.Read(audioBuffer, 0, audioBuffer.Length);

					double[] _audioBuffer = ShortToDouble(audioBuffer);
					bool isAudible = IsAudible(_audioBuffer, MinThreshold, MaxThreshold);

					if (isAudible)
					{
						double freq = FrequencyUtils.FindFundamentalFrequency(_audioBuffer, SampleRate, MinFreq, MaxFreq);
						Frequency.OnNext(Math.Round(freq, 2));
					}
					else
					{
						Frequency.OnNext(0);
					}
				}
			}
			catch
			{
				throw new ArgumentException("FFT crashed");
			}
		}

		public void StopRecording()
		{
			if (audRecorder != null)
			{
				audRecorder.Stop();
				audRecorder.Release();
				audRecorder.Dispose();
			}

		}
		private double[] ShortToDouble(short[] shorts)
		{
			double[] _audioBuffer = new double[shorts.Length];
			for (int i = 0; i < _audioBuffer.Length; i++)
			{
				_audioBuffer[i] = shorts[i];
			}
			return _audioBuffer;
		}

		private bool IsAudible(double[] data, int minThreshold, int maxThreshold)
		{
			double rms = GetRootMeanSquared(data);
			return (rms > minThreshold && maxThreshold > rms);
		}

		private double GetRootMeanSquared(double[] data)
		{
			double ms = 0;
			for (int i = 0; i < data.Length; i++)
			{
				ms += data[i] * data[i];
			}
			ms /= data.Length;
			return Math.Sqrt(ms);
		}
	}
}
