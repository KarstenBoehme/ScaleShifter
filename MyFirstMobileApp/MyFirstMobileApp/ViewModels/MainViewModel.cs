using MyFirstMobileApp.Module;
using MyFirstMobileApp.Module.Fretboard;
using MyFirstMobileApp.Module.KeyPlayer;
using MyFirstMobileApp.Module.Properties;
using Reactive.Bindings;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xamarin.Forms;

namespace MyFirstMobileApp.ViewModels
{
	public class MainViewModel : ReactiveUI.ReactiveObject
	{
		private BehaviorSubject<Model> ModelSubject;
		public Grid FretBoardGrid => ModelSubject.Value.FretBoardGrid.UIGrid;
			
		public ReactiveCommand E1TuneDownCommand { get; }
		public ReactiveCommand BTuneDownCommand { get; }
		public ReactiveCommand GTuneDownCommand { get; }
		public ReactiveCommand ATuneDownCommand { get; }
		public ReactiveCommand DTuneDownCommand { get; }
		public ReactiveCommand ETuneDownCommand { get; }
		public ReactiveCommand AllTuneDownCommand { get; }

		public ReactiveCommand E1TuneUpCommand { get; }
		public ReactiveCommand BTuneUpCommand { get; }
		public ReactiveCommand GTuneUpCommand { get; }
		public ReactiveCommand ATuneUpCommand { get; }
		public ReactiveCommand DTuneUpCommand { get; }
		public ReactiveCommand ETuneUpCommand { get; }
		public ReactiveCommand AllTuneUpCommand { get; }

		public ReactiveProperty<string> E1TuneUpCommandText { get; }
		public ReactiveProperty<string> BTuneUpCommandText { get; }
		public ReactiveProperty<string> GTuneUpCommandText { get; }
		public ReactiveProperty<string> DTuneUpCommandText { get; }
		public ReactiveProperty<string> ATuneUpCommandText { get; }
		public ReactiveProperty<string> ETuneUpCommandText { get; }

		public string TuneDownCommandText => Constants.UpSign;
		public string AllTuneUpCommandText => $"{Constants.DownSign} {Constants.DownSign}";
		public string AllTuneDownCommandText => $"{Constants.UpSign} {Constants.UpSign}";
		public string SettingsCommandText => "...";

		public ReactiveProperty<string> GeneralSettings { get; }

		public MainViewModel(Model model)
		{
			ModelSubject = model.ModelSubject;

			E1TuneDownCommand = new ReactiveCommand();
			E1TuneDownCommand.Subscribe(() => TuneDown(GuitarString.E4));

			BTuneDownCommand = new ReactiveCommand();
			BTuneDownCommand.Subscribe(() => TuneDown(GuitarString.B));

			GTuneDownCommand = new ReactiveCommand();
			GTuneDownCommand.Subscribe(() => TuneDown(GuitarString.G));

			ATuneDownCommand = new ReactiveCommand();
			ATuneDownCommand.Subscribe(() => TuneDown(GuitarString.A));

			DTuneDownCommand = new ReactiveCommand();
			DTuneDownCommand.Subscribe(() => TuneDown(GuitarString.D));

			ETuneDownCommand = new ReactiveCommand();
			ETuneDownCommand.Subscribe(() => TuneDown(GuitarString.E));

			AllTuneDownCommand = new ReactiveCommand();
			AllTuneDownCommand.Subscribe(() => TuneDownAll());

			E1TuneUpCommand = new ReactiveCommand();
			E1TuneUpCommand.Subscribe(() => TuneUp(GuitarString.E4));

			BTuneUpCommand = new ReactiveCommand();
			BTuneUpCommand.Subscribe(() => TuneUp(GuitarString.B));

			GTuneUpCommand = new ReactiveCommand();
			GTuneUpCommand.Subscribe(() => TuneUp(GuitarString.G));

			ATuneUpCommand = new ReactiveCommand();
			ATuneUpCommand.Subscribe(() => TuneUp(GuitarString.A));

			DTuneUpCommand = new ReactiveCommand();
			DTuneUpCommand.Subscribe(() => TuneUp(GuitarString.D));

			ETuneUpCommand = new ReactiveCommand();
			ETuneUpCommand.Subscribe(() => TuneUp(GuitarString.E));

			AllTuneUpCommand = new ReactiveCommand();
			AllTuneUpCommand.Subscribe(() => TuneUpAll());


			E1TuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.E4));
			Observable.Merge(E1TuneUpCommand, E1TuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, ModelSubject)
				.Subscribe(o => E1TuneUpCommandText.Value = GetButtonDescription(GuitarString.E4));

			BTuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.B));
			Observable.Merge(BTuneUpCommand, BTuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, ModelSubject)
				.Subscribe(o => BTuneUpCommandText.Value = GetButtonDescription(GuitarString.B));

			GTuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.G));
			Observable.Merge(GTuneUpCommand, GTuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, ModelSubject)
				.Subscribe(o => GTuneUpCommandText.Value = GetButtonDescription(GuitarString.G));

			ATuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.A));
			Observable.Merge(ATuneUpCommand, ATuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, ModelSubject)
				.Subscribe(o => ATuneUpCommandText.Value = GetButtonDescription(GuitarString.A));

			DTuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.D));
			Observable.Merge(DTuneUpCommand, DTuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, ModelSubject)
				.Subscribe(o => DTuneUpCommandText.Value = GetButtonDescription(GuitarString.D));

			ETuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.E));
			Observable.Merge(ETuneUpCommand, ETuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, ModelSubject)
				.Subscribe(o => ETuneUpCommandText.Value = GetButtonDescription(GuitarString.E));


			GeneralSettings = new ReactiveProperty<string>();
			ModelSubject.Subscribe(m => GeneralSettings.Value = GetDescription(m));
		}

		private string GetDescription(Model model)
		{
			string tuning = $"Tuning: {model.FretBoard.Tuning.Notes}";
			string capo = model.FretBoard.CapoPosition != 0 ? $" | Capo: {model.FretBoard.CapoPosition}" : string.Empty;
			string key = $"Key: {model.FretBoard.Key.GetKeyDiscription()} | ";
			string scale = $"Scale: {model.FretBoard.Scale.Name}";

			return tuning + capo + "\n" + key + scale;
		}

		private string GetButtonDescription(GuitarString guitarString)
		{
			Key standardKey = Keys.GetKey(guitarString);
			Key tunedKey = ModelSubject.Value.FretBoard.FretBoardLayout[guitarString][0].Key;

			return $"{Constants.DownSign} {standardKey.GetStepsFromStandard(tunedKey)}";
		}

		private void TuneDown(GuitarString guitarString)
		{
			ModelSubject.Value.FretBoard.TuneDown(guitarString);
			ModelSubject.Value.UpdateFretboardUIGrid(guitarString);
		}

		private void TuneDownAll()
		{
			EnumCollectionCreator<GuitarString>.GetEnumCollection().ToList().ForEach(s => ModelSubject.Value.FretBoard.TuneDown(s));
			ModelSubject.Value.UpdateFretboardUIGrid();
		}

		private void TuneUp(GuitarString guitarString)
		{
			ModelSubject.Value.FretBoard.TuneUp(guitarString);
			ModelSubject.Value.UpdateFretboardUIGrid(guitarString);
		}

		private void TuneUpAll()
		{
			EnumCollectionCreator<GuitarString>.GetEnumCollection().ToList().ForEach(s => ModelSubject.Value.FretBoard.TuneUp(s));
			ModelSubject.Value.UpdateFretboardUIGrid();
		}
	}
}
