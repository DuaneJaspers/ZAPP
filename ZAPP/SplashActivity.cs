using System;
using Android.Content;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace ZAPP
{
    using System.Threading;
    using Android.App;
    using Android.OS;

    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true,
                                             NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _database db = new _database(this);
            //db.login();

            //Thread.Sleep(10000);

            StartActivity(typeof(Home));
        }

    }
}