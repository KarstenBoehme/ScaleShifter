using Android;
using Android.App;
using Android.Content.PM;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Threading.Tasks;

namespace MyFirstMobileApp.Droid
{
	[Activity(Label = "Scale Shifter", 
        Icon = "@mipmap/icon", 
        Theme = "@style/MainTheme", 
        MainLauncher = true, 
        ScreenOrientation = ScreenOrientation.Landscape,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.SetFlags("RadioButton_Experimental");

            Rg.Plugins.Popup.Popup.Init(this, savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

             GetAudioRecorderPermission();
            LoadApplication(new App());

            Window.SetStatusBarColor(Android.Graphics.Color.ParseColor("#2E8364"));
            Window.SetTitleColor(Android.Graphics.Color.ParseColor("#2E8364"));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void GetAudioRecorderPermission()
        {
            const int RequestLocationId = 0;

            bool sdk23 = (int)Build.VERSION.SdkInt >= 23;
            bool permissionDenied = ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) == Permission.Denied;

            if (sdk23 && permissionDenied)
            {
                //TODO logger
                RequestPermissions(new string[] { Manifest.Permission.RecordAudio }, RequestLocationId);
            }
        }
    }
}