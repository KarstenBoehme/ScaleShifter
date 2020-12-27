using MyFirstMobileApp.Module;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MyFirstMobileApp
{
    public partial class App : Application
    {
        private Model model;
        public App()
        {
            InitializeComponent();
            model = new Model();
            MainPage = new MainPage(model);
        }

        protected override void OnStart()
        {
            AppPreferences.Load(model);
        }

        protected override void OnSleep()
        {
            AppPreferences.Save(model);
        }

        protected override void OnResume()
        {
        }
    }
}
