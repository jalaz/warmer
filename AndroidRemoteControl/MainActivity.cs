using System;
using System.Net;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net.Http;
using Android.Net.Wifi;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Java.Util.Logging;

namespace AndroidRemoteControl
{
    [Activity(Label = "AndroidRemoteControl", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : Activity
    {
        int count = 1;
        private Button button;
        private bool isTunrOn;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            button = FindViewById<Button>(Resource.Id.MyButton);
            button.Text = "togle state";

            //button.Click += delegate { button.Text = string.Format("{0} clicks!", count++); };
            button.Click += OnButtonClick;
        }

        private void OnButtonClick(object sender, EventArgs e)
        {
            button.Text = "updating state...";
            //EnableWifi();
            var client = new WebClient();
            //client.BaseAddress = @"http://localhost:54111";
            var deviceUrl = "http://192.168.4.1/device?turnOn=";
            deviceUrl += isTunrOn ? "0" : "1";
            var response = client.DownloadString(new Uri(deviceUrl));
            if (response.Contains("OFF"))
                isTunrOn = false;
            else if (response.Contains("ON"))
            {
                isTunrOn = true;
            }

            button.Text = isTunrOn ? "ON" : "OFF";

        }

        private void EnableWifi()
        {
            string networkSSID = "test123";
            string networkPass = "test_123";

            WifiConfiguration wifiConfig = new WifiConfiguration();
            wifiConfig.Ssid = string.Format("\"{0}\"", networkSSID);
            wifiConfig.PreSharedKey = string.Format("\"{0}\"", networkPass);

            WifiManager wifiManager = (WifiManager)Application.Context.GetSystemService(Context.WifiService);

            //var permissionCheck = CheckSelfPermission(Manifest.Permission.ChangeWifiState.ToString());

            //if (permissionCheck == Permission.Denied)
            //{
            //    Logger.Global.Log(Level.Warning,
            //        $"There is npo {Manifest.Permission.ChangeWifiState.ToString()} permission");
            //    return;
            //}
            // Use ID
            int netId = wifiManager.AddNetwork(wifiConfig);
            wifiManager.Disconnect();
            wifiManager.EnableNetwork(netId, true);
            wifiManager.Reconnect();
        }
    }
}

