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
	public partial class TuningListEditorPage : Rg.Plugins.Popup.Pages.PopupPage
	{
		private DescriptionInputPage _DiscriptionInputPage;
		public TuningListEditorPage(Model model)
		{
			InitializeComponent();
			BindingContext = new TuningListEditorViewModel(model);
			_DiscriptionInputPage = new DescriptionInputPage();
		}

		private async void OnAdd(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PushAsync(_DiscriptionInputPage);
		}
		private async void OnCancel(object sender, EventArgs e)
		{
			await PopupNavigation.Instance.PopAsync();
			tuningListView.SelectedItem = null;
		}
	}
}
