using MyFirstMobileApp.Module;
using MyFirstMobileApp.Module.Properties;
using MyFirstMobileApp.Module.Tuner;
using Reactive.Bindings;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace MyFirstMobileApp.ViewModels
{
	public class TunerViewModel : ReactiveUI.ReactiveObject
	{
		public BehaviorSubject<bool> IsActivatedSubject { get; set; }

		private BehaviorSubject<Model> ModelSubject;
		private GuitarTuner GuitarTuner;

		public ReactiveProperty<string> Note { get; }
		public ReactiveProperty<string> Frequency { get; }

		//public ReactiveProperty<double> MinFrequency { get; }
		//public ReactiveProperty<double> MaxFrequency { get; }
		public ReactiveProperty<double> Buffer { get; }

		public TunerViewModel(Model model)
		{
			IsActivatedSubject = new BehaviorSubject<bool>(false);
			ModelSubject = model.ModelSubject;
			GuitarTuner = new GuitarTuner();

			IsActivatedSubject.Subscribe(isActivated =>
			{
				if (isActivated)
				{
					Task.Run(() => GuitarTuner.RecordAudio());
				}
				else
				{
					GuitarTuner.StopRecording();
				}
			});

			Note = new ReactiveProperty<string>("-");
			Frequency = new ReactiveProperty<string>("Hz");

			//MinFrequency = new ReactiveProperty<double>(0);
			//MaxFrequency = new ReactiveProperty<double>(0);

			GuitarTuner.Frequency.Throttle(TimeSpan.FromMilliseconds(50)).Subscribe(freq =>
			{
				this.Frequency.Value = freq.ToString("0.00") + " Hz";

				if (freq == 0)
				{
					this.Note.Value = "-";
				}
				else
				{
					Freq _freq = ModelSubject.Value.DataBaseHandler.FrequencyCollection.Single(f => f.IsWithinPlusMinus50Cent(freq));
					this.Note.Value = $"{Keys.ListOfKeys.Single(k => k.ToString().Equals(_freq.Note)).GetKeyDiscription()} ({_freq.Frequency} Hz)";
				}
			});

#if DEBUG
			Buffer = new ReactiveProperty<double>(0);
			GuitarTuner.Buffer.Throttle(TimeSpan.FromMilliseconds(100)).Subscribe(b => Buffer.Value = Math.Round(b, 5));
#endif
		}
	}
}
