using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using CommunityToolkit.Mvvm.Messaging;
using PrototypeDataWedgeClientHosted.Platforms.Android.Resources;

namespace PrototypeDataWedgeClientHosted
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    [IntentFilter(new[] { "com.Aaron.datawedge.xamarin.ACTION" }, Categories = new[] { Intent.CategoryDefault })]
    public class MainActivity : MauiAppCompatActivity
    {
        private bool buttonClicked = false;
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DWUtils.CreateDWProfile(this);

            WeakReferenceMessenger.Default.Register<string>(this, (r, li) =>
            {
                MainThread.BeginInvokeOnMainThread(() => {
                    if (li == "11")
                    {
                        // Activate scanner
                        Intent dwIntent = new Intent();
                        dwIntent.SetAction("com.symbol.datawedge.api.ACTION");
                        dwIntent.PutExtra("com.symbol.datawedge.api.SOFT_SCAN_TRIGGER", "START_SCANNING");
                        SendBroadcast(dwIntent);
                    }
                    else if (li == "22")
                    {
                        // Disable scanner
                        Intent dwIntent = new Intent();
                        dwIntent.SetAction("com.symbol.datawedge.api.ACTION");
                        dwIntent.PutExtra("com.symbol.datawedge.api.SOFT_SCAN_TRIGGER", "STOP_SCANNING");
                        SendBroadcast(dwIntent);
                    }

                });
            });
        }

        /// <summary>
        /// Receives intents
        /// </summary>
        /// <param name="intent">Intent to receive</param>
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            String decodedData = intent.GetStringExtra(Resources.GetString(Resource.String.datawedge_intent_key_data));
            String decodedLabelType = intent.GetStringExtra(Resources.GetString(Resource.String.datawedge_intent_key_label_type));
            String scan = decodedData + " [" + decodedLabelType + "]\n\n";

            // Send data to page
            WeakReferenceMessenger.Default.Send("Data from scan: " + scan);
        }
    }
}
