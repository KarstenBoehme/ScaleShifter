using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;

namespace MyFirstMobileApp
{
    public partial class MainPage : ContentPage
    {
        private TuningListEditorPage _ListEditorTuning;
        private KeyListEditorPage _ListEditorKey;
        private ScaleListEditorPage _ListEditorScale;
        private SettingsPage _SettingsPage;
        public MainPage(Model model)
        {
            InitializeComponent();
            BindingContext = new MainViewModel(model);

            _ListEditorTuning = new TuningListEditorPage();
            _ListEditorKey = new KeyListEditorPage();
            _ListEditorScale = new ScaleListEditorPage();
            _SettingsPage = new SettingsPage(model);
        } 

        private async void OnOpenTuning(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_ListEditorTuning);
        }

        private async void OnOpenKey(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_ListEditorKey);
        }

        private async void OnOpenScale(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_ListEditorScale);
        }
        private async void OnOpenSettings(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_SettingsPage);
        }
    }
}
