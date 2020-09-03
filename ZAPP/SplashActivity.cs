using System;
using Android.Content;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace ZAPP
{
    using System.Threading;
    using System.Threading.Tasks;
    using Android.App;
    using Android.OS;

    [Activity(Theme = "@style/Theme.Splash", MainLauncher = true,
                                             NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

        }

        protected override void OnResume()
        {
            base.OnResume();
            Task StartUpWork = new Task( () => { Startup();  });
            StartActivity(typeof(Home));
    }

        async void Startup()
        {

            _database db = new _database(this);
            await Task.Delay(200);
            //db.login();


            
        }

    }
}