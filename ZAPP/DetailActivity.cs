using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Collections;
using ZAPP.Records;
using Android.Content.Res;
using Android.Support.V4.View;
using Android.Support.V4.App;

namespace ZAPP
{
    [Activity(Label = "DetailActivity")]
    public class DetailActivity : Android.Support.V4.App.FragmentActivity
    {
        AppointmentRecord appointmentRecord;
        string appointmentId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
           

            base.OnCreate(savedInstanceState);
        
            this.appointmentId = Intent.GetStringExtra("ID");
            _database db = new _database(this);
            appointmentRecord = db.getAppointmentById(appointmentId);

            
            SetContentView(Resource.Layout.Detail);

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewPagerDetails);
            viewPager.Adapter = new DetailPagerAdapter(this, this.SupportFragmentManager, appointmentRecord);


            Button aanmeldButton = FindViewById<Button>(Resource.Id.aanmeldButton);
            if (Singleton.currentlyWorking != null)
            {
                if (Singleton.currentlyWorking != appointmentId)
                {
                aanmeldButton.Enabled = false;

                } else
                {
                    aanmeldButton.Text = Resources.GetString(Resource.String.StopWorking);
                    if (!Singleton.tasksComplete)
                        aanmeldButton.Enabled = false;
                }
            }

            aanmeldButton.Click += delegate
            {
                toggleAanmeldButton(aanmeldButton);
            };

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.detail_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        protected void toggleAanmeldButton(Button aanmeldButton)
        {
            Resources res = Resources;
            _database db = new _database(this);
            // aanmeldbutton
            if (aanmeldButton.Text == res.GetString(Resource.String.StartWorking))
            {
                // send time to to api (maybe in updateTimeForAppointment?)
                db.updateTimeForAppointment(int.Parse(appointmentId), "time_start");
                Singleton.currentlyWorking = appointmentId;
                aanmeldButton.Text = res.GetString(Resource.String.StopWorking);
                aanmeldButton.Enabled = false;


                // reload the tasklist to enable checkboxes
                ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewPagerDetails);
                viewPager.Adapter = new DetailPagerAdapter(this, this.SupportFragmentManager, appointmentRecord);


            }
            // afmeldbutton hit
            else
            {
                db.updateTimeForAppointment(int.Parse(appointmentId), "time_finish");
                // send time to to api (maybe in updateTimeForAppointment?)
                // TODO
                //
                Singleton.currentlyWorking = null;

                aanmeldButton.Enabled = false;
                Toast.MakeText(this, "Appointment finished!", ToastLength.Long).Show();
                // goes back to mainActivity
                Finish();
            }
        }
    }

}