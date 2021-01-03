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
				short[] audioBuffer = new short[minBufferSize];

				audRecorder = new AudioRecord(AudioSource.VoiceRecognition, SampleRate, ChannelIn.Mono, Encoding.Pcm16bit, audioBuffer.Length);
				audRecorder.StartRecording();

				while (true)
				{
					audRecorder.Read(audioBuffer, 0, audioBuffer.Length);
					
					int threshold = Frequency.Value > 150 ? 20 :
									Frequency.Value > 250 ? 20 : 
									30;

					int averageWidth = Frequency.Value > 150 ? 50 :
										Frequency.Value > 250 ? 10 :
										100;

					double[] _audioBufferAveraged = RollingAverage(ShortToDouble(audioBuffer), averageWidth).ToArray();

					bool isSignal = IsSignal(_audioBufferAveraged, threshold);

					if (isSignal)
					{
						double freq = FrequencyUtils.FindFundamentalFrequency(_audioBufferAveraged, SampleRate, MinFreq, MaxFreq);
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
				//TODO implement logger
			}
		}

		public void StopRecording()
		{
			if (audRecorder != null)
			{
				audRecorder.Stop();
				audRecorder.Release();
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

		private bool IsSignal(double[] buffer, int threshold)
		{
			for (int index = 0; index < buffer.Length; index++)
			{
				Buffer.OnNext(buffer[index]);

				if (Math.Abs(buffer[index]) > threshold)
				{
					return true;
				}
			}
			return false;
		}

		private double[] RollingAverageLinq(double[] buffer, int width)
		{
			return Enumerable.Range(0, 1 + buffer.Length - width)
										.Select(i => buffer.Skip(i).Take(width).Average())
										.ToArray();
		}

		private IEnumerable<double> RollingAverage(double[] buffer, int length)
		{
			var queue = new Queue<double>(length);
			double sum = 0;
			foreach (int i in buffer)
			{
				if (queue.Count == length)
				{
					yield return sum / length;
					sum -= queue.Dequeue();
				}
				sum += i;
				queue.Enqueue(i);
			}
			yield return sum / length;
		}
	}
}
