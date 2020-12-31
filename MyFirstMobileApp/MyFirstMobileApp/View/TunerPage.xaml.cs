using MyFirstMobileApp.Module;
using MyFirstMobileApp.ViewModels;
using ReactiveUI;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyFirstMobileApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TunerPage : Rg.Plugins.Popup.Pages.PopupPage
	{
		public TunerPage(Model model)
		{
			InitializeComponent();
			BindingContext = new TunerViewModel(model);
		}

		private async void OnClose(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
		}
	}
}
