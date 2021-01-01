using MyFirstMobileApp.Module;
using MyFirstMobileApp.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace MyFirstMobileApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TunerPage : Rg.Plugins.Popup.Pages.PopupPage
	{
		private TunerViewModel ViewModel { get; set; }
		public TunerPage(Model model)
		{
			InitializeComponent();

			ViewModel = new TunerViewModel(model);
			BindingContext = ViewModel;
		}

		protected override void OnAppearing()
		{
			ViewModel.IsActivatedSubject.OnNext(true);
		}

		protected override void OnDisappearing()
		{
			ViewModel.IsActivatedSubject.OnNext(false);
		}

		private async void OnClose(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
		}
	}
}
