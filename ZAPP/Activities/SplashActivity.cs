using System;
using Android.Content;
using Android.Views;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;

namespace ZAPP.Activities
{
    using System.Threading;
    using System.Threading.Tasks;
    using Android.App;
    using Android.OS;
    using ZAPP.Records;

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
            StartUpWork.Start();
            //StartActivity(typeof(Home));
            //StartActivity(typeof(LoginActivity));

        }

        async void Startup()
        {

            _database db = new _database(this);
            string UserToken = Singleton.userToken;
            if (String.IsNullOrEmpty(Singleton.userToken))
            {
                db.getUserToken();
                if (String.IsNullOrEmpty(Singleton.userToken))
                {
                    StartActivity(typeof(LoginActivity));
                    return;
                }
            }
            await Task.Delay(200);
            StartActivity(typeof(Home));
            //db.login();



        }

    }
}