using MyFirstMobileApp.Module;
using Reactive.Bindings;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xamarin.Essentials;

namespace MyFirstMobileApp.ViewModels
{
	public class SettingsViewModel : ReactiveUI.ReactiveObject
	{
		private BehaviorSubject<Model> ModelSubject;

		public ReactiveProperty<KeyDisplayingSettings> DisplayingSettings { get; }
		public ReactiveProperty<SemiStepSettings> SemiSteps { get; }

		public ReactiveProperty<int> CapoPosition { get; }
		public ReactiveCommand CapoPlusCommand { get; }
		public ReactiveCommand CapoMinusCommand { get; }

		public ReactiveCommand ConfirmCommand { get; }
		public ReactiveCommand CancelCommand { get; }

		public SettingsViewModel(Model model)
		{
			ModelSubject = model.ModelSubject;

			DisplayingSettings = new ReactiveProperty<KeyDisplayingSettings>(Settings.KeyDisplayingSettings);
			SemiSteps = new ReactiveProperty<SemiStepSettings>(Settings.SemiStepSettings);
			CapoPosition = new ReactiveProperty<int>(ModelSubject.Value.FretBoard.CapoPosition);
			CapoMinusCommand = new ReactiveCommand();
			CapoPlusCommand = new ReactiveCommand();
			ConfirmCommand = new ReactiveCommand();
			CancelCommand = new ReactiveCommand();

			CapoMinusCommand.Subscribe(() => 
			{
				if(CapoPosition.Value > 0)
					CapoPosition.Value--;
			});

			CapoPlusCommand.Subscribe(() =>
			{
				if (CapoPosition.Value < 12)
					CapoPosition.Value++;
			});

			ConfirmCommand
				.WithLatestFrom(Observable.CombineLatest(CapoPosition, DisplayingSettings, SemiSteps, (capo, display, semi) => (capo, display, semi)), (_, t) => t)
				.Subscribe(tuple =>
			{
				ModelSubject.Value.FretBoard.CapoPosition = tuple.capo;
				Settings.KeyDisplayingSettings = tuple.display;
				Settings.SemiStepSettings = tuple.semi;
				ModelSubject.Value.RefreshModel();
			});

			CancelCommand.Subscribe(() =>
			{
				DisplayingSettings.Value = Settings.KeyDisplayingSettings;
				SemiSteps.Value = Settings.SemiStepSettings;
				CapoPosition.Value = ModelSubject.Value.FretBoard.CapoPosition;
			});
		}
	}
}
