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

		public ReactiveProperty<double> SetFrequency { get; }
		public ReactiveProperty<double> RecordedFrequency { get; }

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
			RecordedFrequency = new ReactiveProperty<double>();
			SetFrequency = new ReactiveProperty<double>();

			GuitarTuner.Frequency.Throttle(TimeSpan.FromMilliseconds(25)).Subscribe(freq =>
			{
				this.RecordedFrequency.Value = freq;
				this.Frequency.Value = freq.ToString("0.00") + " Hz";

				if (freq == 0)
				{
					this.Note.Value = "-";
				}
				else
				{
					Freq _freq = ModelSubject.Value.DataBaseHandler.FrequencyCollection.Single(f => f.IsWithinPlusMinus50Cent(freq));
					this.Note.Value = Keys.ListOfKeys.Single(k => k.ToString().Equals(_freq.Note)).GetKeyDiscription();
					this.SetFrequency.Value = _freq.Frequency;
				}
			});
		}
	}
}
