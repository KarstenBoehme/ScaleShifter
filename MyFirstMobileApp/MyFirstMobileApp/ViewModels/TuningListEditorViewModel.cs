using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xamarin.Essentials;

namespace MyFirstMobileApp
{
	public class TuningListEditorViewModel : ReactiveUI.ReactiveObject
	{
		private BehaviorSubject<Model> ModelSubject;

		public ReactiveProperty<string> CurrentTuning { get; }
		public ReactiveProperty<Tuning> SelectedTuning { get; }
		public ReactiveProperty<string> TuningDescription { get; }
		public ObservableCollection<Tuning> TuningCollection
		{
			get
			{
				return ModelSubject.Value.DataBaseHandler.TuningCollection;
			}
		}
		public ReactiveCommand AddTuningCommand { get; }
		public ReactiveCommand DeleteTuningCommand { get; }
		public ReactiveCommand SelectTuningCommand { get; }
		public ReactiveProperty<bool> IsEnabledDeleteTuningCommand { get; }

		public TuningListEditorViewModel(Model model)
		{
			ModelSubject = model.ModelSubject;

			SelectedTuning = new ReactiveProperty<Tuning>();
			TuningDescription = new ReactiveProperty<string>(string.Empty);

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

			IsEnabledDeleteTuningCommand = new ReactiveProperty<bool>();
			SelectedTuning.Subscribe(t => IsEnabledDeleteTuningCommand.Value = t != null && !t.IsLocked());

			CurrentTuning = new ReactiveProperty<string>();
			ModelSubject.Subscribe(m => CurrentTuning.Value =
				"Current Tuning: " + m.FretBoard.Tuning.Notes);
		}

		private void AddTuningToDB(string description)
		{
			Tuning newTuning = (Tuning)ModelSubject.Value.FretBoard.Tuning.Clone();
			newTuning.Description = description;
			ModelSubject.Value.DataBaseHandler.AddTuningToDB(newTuning);
		}
	}
}
