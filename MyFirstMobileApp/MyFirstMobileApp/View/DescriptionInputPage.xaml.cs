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
	public partial class DescriptionInputPage : Rg.Plugins.Popup.Pages.PopupPage
	{
		public DescriptionInputPage()
		{
			InitializeComponent();
		}

		private async void OnClose(object sender, EventArgs e)
		{
			descriptionEntry.Text = null;
			await PopupNavigation.Instance.PopAsync();
		}
	}
}
