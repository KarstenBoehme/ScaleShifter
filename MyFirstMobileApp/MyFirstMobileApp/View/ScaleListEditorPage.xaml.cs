using MyFirstMobileApp.Module;
using MyFirstMobileApp.ViewModels;
using Rg.Plugins.Popup.Services;
using System;
using Xamarin.Forms.Xaml;

namespace MyFirstMobileApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScaleListEditorPage : Rg.Plugins.Popup.Pages.PopupPage
	{
		public ScaleListEditorPage(Model model)
		{
			InitializeComponent();
			BindingContext = new ScaleListEditorViewModel(model);
		}

		private async void OnCancel(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
		}
	}
}
