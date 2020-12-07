using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xamarin.Forms;

namespace MyFirstMobileApp
{
	public class MainViewModel : ReactiveUI.ReactiveObject
	{
		private BehaviorSubject<Model> ModelSubject;
		public Grid FretBoardGrid => ModelSubject.Value.FretBoardGrid.UIGrid;

		public ReactiveProperty<Key> SelectedKey { get; }
		public ReactiveProperty<Scale> SelectedScale { get; }
		public ReactiveProperty<Tuning> SelectedTuning { get; }
		public ReactiveProperty<string> TuningDescription { get; }

		public List<Key> KeyCollection
		{
			get
			{
				return Keys.ListOfKeys;
			}
		}
		public ObservableCollection<Scale> ScaleCollection
		{
			get
			{
				return ModelSubject.Value.DataBaseHandler.ScaleCollection;
			}
		}
		public ObservableCollection<Tuning> TuningCollection
		{
			get
			{
				return ModelSubject.Value.DataBaseHandler.TuningCollection;
			}
		}

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

		public ReactiveCommand AddTuningCommand { get; }
		public ReactiveCommand DeleteTuningCommand { get; }
		public ReactiveCommand SelectTuningCommand { get; }
		public ReactiveCommand SelectKeyCommand { get; }
		public ReactiveCommand SelectScaleCommand { get; }

		public ReactiveProperty<bool> IsEnabledDeleteTuningCommand { get; }
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

		public ReactiveProperty<string> CurrentTuning { get; }
		public ReactiveProperty<string> CurrentKey { get; }
		public ReactiveProperty<string> CurrentScale { get; }
		public ReactiveProperty<string> GeneralSettings { get; }

		public MainViewModel(Model model)
		{
			ModelSubject = model.ModelSubject;

			SelectedKey = new ReactiveProperty<Key>(ModelSubject.Value.FretBoard.Key);
			SelectedScale = new ReactiveProperty<Scale>(ModelSubject.Value.FretBoard.Scale);
			SelectedTuning = new ReactiveProperty<Tuning>();
			TuningDescription = new ReactiveProperty<string>(string.Empty);

			E1TuneDownCommand = new ReactiveCommand();
			E1TuneDownCommand.Subscribe(() => TuneDown(GuitarString.E1));

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
			E1TuneUpCommand.Subscribe(() => TuneUp(GuitarString.E1));

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

			AddTuningCommand = new ReactiveCommand();
			AddTuningCommand.WithLatestFrom(TuningDescription, (_, d) => d).Subscribe(d => AddTuningToDB(d));

			DeleteTuningCommand = new ReactiveCommand();
			DeleteTuningCommand.Subscribe(() => ModelSubject.Value.DataBaseHandler.RemoveTuningFromDB(SelectedTuning.Value));

			SelectTuningCommand = new ReactiveCommand();
			SelectTuningCommand
				.WithLatestFrom(SelectedTuning, (_, t) => (t))
				.Subscribe(t =>
				{
					SelectedTuning.Value = t;
					ModelSubject.Value.FretBoard.SetTuning(t);
					ModelSubject.Value.UpdateFretboardUIGrid();
				});

			SelectScaleCommand = new ReactiveCommand();
			SelectScaleCommand
				.WithLatestFrom(SelectedScale, (_, s) => s)
				.WithLatestFrom(SelectedKey, (s, k) => (s, k))
				.Subscribe(tuple =>
				{
					ModelSubject.Value.FretBoard.SetScale(tuple.s, tuple.k);
					ModelSubject.Value.UpdateFretboardUIGrid();
				});

			SelectKeyCommand = new ReactiveCommand();
			SelectKeyCommand
				.WithLatestFrom(SelectedScale, (_, s) => s)
				.WithLatestFrom(SelectedKey, (s, k) => (s, k))
				.Subscribe(tuple =>
				{
					ModelSubject.Value.FretBoard.SetScale(tuple.s, tuple.k);
					ModelSubject.Value.UpdateFretboardUIGrid();
				});

			IsEnabledDeleteTuningCommand = new ReactiveProperty<bool>();
			SelectedTuning.Subscribe(t => IsEnabledDeleteTuningCommand.Value = t != null && !t.IsLocked());

			E1TuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.E1));
			Observable.Merge(E1TuneUpCommand, E1TuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, SelectTuningCommand)
				.Subscribe(o => E1TuneUpCommandText.Value = GetButtonDescription(GuitarString.E1));

			BTuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.B));
			Observable.Merge(BTuneUpCommand, BTuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, SelectTuningCommand)
				.Subscribe(o => BTuneUpCommandText.Value = GetButtonDescription(GuitarString.B));

			GTuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.G));
			Observable.Merge(GTuneUpCommand, GTuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, SelectTuningCommand)
				.Subscribe(o => GTuneUpCommandText.Value = GetButtonDescription(GuitarString.G));

			ATuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.A));
			Observable.Merge(ATuneUpCommand, ATuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, SelectTuningCommand)
				.Subscribe(o => ATuneUpCommandText.Value = GetButtonDescription(GuitarString.A));

			DTuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.D));
			Observable.Merge(DTuneUpCommand, DTuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, SelectTuningCommand)
				.Subscribe(o => DTuneUpCommandText.Value = GetButtonDescription(GuitarString.D));

			ETuneUpCommandText = new ReactiveProperty<string>(GetButtonDescription(GuitarString.E));
			Observable.Merge(ETuneUpCommand, ETuneDownCommand, AllTuneDownCommand, AllTuneUpCommand, SelectTuningCommand)
				.Subscribe(o => ETuneUpCommandText.Value = GetButtonDescription(GuitarString.E));

			GeneralSettings = new ReactiveProperty<string>();
			ModelSubject.Subscribe(m => GeneralSettings.Value = GetDescription(m));

			CurrentTuning = new ReactiveProperty<string>();
			ModelSubject.Subscribe(m => CurrentTuning.Value =
				"Current Tuning: " + m.FretBoard.Tuning.Notes);

			CurrentKey = new ReactiveProperty<string>();
			ModelSubject.Subscribe(m => CurrentKey.Value =
				"Current Key: " + m.FretBoard.Key.GetKeyDiscription());

			CurrentScale = new ReactiveProperty<string>();
			ModelSubject.Subscribe(m => CurrentScale.Value =
				"Current Scale: " + m.FretBoard.Scale.Name);
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
			return $"{Constants.DownSign} {ModelSubject.Value.FretBoard.GetStepsFromStandard(guitarString)}";
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

		private void AddTuningToDB(string description)
		{
			Tuning newTuning = (Tuning)ModelSubject.Value.FretBoard.Tuning.Clone();
			newTuning.Description = description;
			ModelSubject.Value.DataBaseHandler.AddTuningToDB(newTuning);
		}
	}
}
