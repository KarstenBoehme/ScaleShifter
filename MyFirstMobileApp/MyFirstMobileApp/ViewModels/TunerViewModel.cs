using MyFirstMobileApp.Module;
using MyFirstMobileApp.Module.Tuner;
using Reactive.Bindings;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace MyFirstMobileApp.ViewModels
{
	public class TunerViewModel : ReactiveUI.ReactiveObject
	{
		private BehaviorSubject<Model> ModelSubject;
		public GuitarTuner GuitarTuner { get; }

		public ReactiveProperty<string> Frequency { get; }
		public ReactiveCommand CloseCommand { get; }

		public TunerViewModel(Model model)
		{
			ModelSubject = model.ModelSubject;
			GuitarTuner = new GuitarTuner();

			Task.Run(() => GuitarTuner.RecordAudio());

			Frequency = new ReactiveProperty<string>("Hz");
			CloseCommand = new ReactiveCommand();

			GuitarTuner.Frequency.Subscribe(freq => this.Frequency.Value = $"{freq} Hz");

			CloseCommand.Subscribe(() => GuitarTuner.audRecorder.Release());
		}
	}
}
