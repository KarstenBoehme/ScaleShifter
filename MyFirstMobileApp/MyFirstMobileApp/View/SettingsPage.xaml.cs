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
	public partial class SettingsPage : Rg.Plugins.Popup.Pages.PopupPage
	{
		public SettingsPage(Model model)
		{
			InitializeComponent();
			BindingContext = new SettingsViewModel(model);
		}

		private async void OnClose(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
		}
	}
}
