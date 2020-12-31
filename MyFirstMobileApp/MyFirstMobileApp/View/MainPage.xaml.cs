using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Rg.Plugins.Popup.Services;
using MyFirstMobileApp.Module;
using MyFirstMobileApp.ViewModels;

namespace MyFirstMobileApp
{
    public partial class MainPage : ContentPage
    {
        private TuningListEditorPage _ListEditorTuning;
        private KeyListEditorPage _ListEditorKey;
        private ScaleListEditorPage _ListEditorScale;
        private SettingsPage _SettingsPage;
        private TunerPage _TunerPage;
        public MainPage(Model model)
        {
            InitializeComponent();
            BindingContext = new MainViewModel(model);

            _ListEditorTuning = new TuningListEditorPage(model);
            _ListEditorKey = new KeyListEditorPage(model);
            _ListEditorScale = new ScaleListEditorPage(model);
            _SettingsPage = new SettingsPage(model);
            _TunerPage = new TunerPage(model);
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
        private async void OnOpenTuner(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(_TunerPage);
        }
    }
}
