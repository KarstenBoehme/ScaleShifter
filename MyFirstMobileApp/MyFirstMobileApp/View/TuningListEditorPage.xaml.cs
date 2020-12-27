using MyFirstMobileApp.Module;
using MyFirstMobileApp.ViewModels;
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
		private TuningListEditorViewModel tuningListEditorViewModel;
		private DescriptionInputPage _DiscriptionInputPage;
		public TuningListEditorPage(Model model)
		{
			InitializeComponent();
			tuningListEditorViewModel = new TuningListEditorViewModel(model);
			BindingContext = tuningListEditorViewModel;

			_DiscriptionInputPage = new DescriptionInputPage();
			_DiscriptionInputPage.BindingContext = tuningListEditorViewModel;
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
