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
	public partial class KeyListEditorPage : Rg.Plugins.Popup.Pages.PopupPage
	{
		public KeyListEditorPage(Model model)
		{
			InitializeComponent();
			BindingContext = new KeyListEditorViewModel(model);
		}

		private async void OnCancel(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
		}
	}
}
