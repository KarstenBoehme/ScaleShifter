using MyFirstMobileApp.Module;
using MyFirstMobileApp.Module.Properties;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xamarin.Essentials;

namespace MyFirstMobileApp.ViewModels
{
	public class KeyListEditorViewModel : ReactiveUI.ReactiveObject
	{
		private BehaviorSubject<Model> ModelSubject;
		public ReactiveProperty<Key> SelectedKey { get; }
		public ReactiveProperty<string> CurrentKey { get; }
		public ReactiveCommand SelectKeyCommand { get; }
		public List<Key> KeyCollection
		{
			get
			{
				return Keys.ListOfKeys;
			}
		}

		public KeyListEditorViewModel(Model model)
		{
			ModelSubject = model.ModelSubject;
			SelectedKey = new ReactiveProperty<Key>(ModelSubject.Value.FretBoard.Key);

			SelectKeyCommand = new ReactiveCommand();
			SelectKeyCommand
				.WithLatestFrom(ModelSubject, (_, m) => m)
				.WithLatestFrom(SelectedKey, (m, k) => (m, k))
				.Subscribe(tuple =>
				{
					ModelSubject.Value.FretBoard.SetScale(tuple.m.FretBoard.Scale, tuple.k);
					ModelSubject.Value.UpdateFretboardUIGrid();
				});

			CurrentKey = new ReactiveProperty<string>();
			ModelSubject.Subscribe(m => CurrentKey.Value =
				"Current Key: " + m.FretBoard.Key.GetKeyDiscription());
		}
	}
}
