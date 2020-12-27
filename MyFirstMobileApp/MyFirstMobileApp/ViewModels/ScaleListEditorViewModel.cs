using MyFirstMobileApp.Module;
using MyFirstMobileApp.Module.Properties;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xamarin.Essentials;

namespace MyFirstMobileApp.ViewModels
{
	public class ScaleListEditorViewModel : ReactiveUI.ReactiveObject
	{
		private BehaviorSubject<Model> ModelSubject;
		public ReactiveProperty<Scale> SelectedScale { get; }
		public ReactiveProperty<string> CurrentScale { get; }
		public ReactiveCommand SelectScaleCommand { get; }
		public ObservableCollection<Scale> ScaleCollection
		{
			get
			{
				return ModelSubject.Value.DataBaseHandler.ScaleCollection;
			}
		}

		public ScaleListEditorViewModel(Model model)
		{
			ModelSubject = model.ModelSubject;
			SelectedScale = new ReactiveProperty<Scale>(ModelSubject.Value.FretBoard.Scale);

			SelectScaleCommand = new ReactiveCommand();
			SelectScaleCommand
				.WithLatestFrom(SelectedScale, (_, s) => s)
				.WithLatestFrom(ModelSubject, (s, m) => (s, m))
				.Subscribe(tuple =>
				{
					ModelSubject.Value.FretBoard.SetScale(tuple.s, tuple.m.FretBoard.Key);
					ModelSubject.Value.UpdateFretboardUIGrid();
				});

			CurrentScale = new ReactiveProperty<string>();
			ModelSubject.Subscribe(m => CurrentScale.Value =
				"Current Scale: " + m.FretBoard.Scale.Name);
		}
	}
}
