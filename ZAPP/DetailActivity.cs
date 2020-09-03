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
        ListView listView;
        List<TaskRecord> records;
        ArrayList tasks;
        string appointmentId;
        bool workingHere = false;
        bool workingComplete = false;
        bool workingSomewhere = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
           

            base.OnCreate(savedInstanceState);
        
            this.appointmentId = Intent.GetStringExtra("ID");
            _database db = new _database(this);
            AppointmentRecord appointmentRecord = db.getAppointmentById(appointmentId);

            this.workingHere = Intent.GetBooleanExtra("working", false);

            // Check if working somewhere else to prevent working at two places
            if (!workingHere)
                workingSomewhere = Intent.GetBooleanExtra("workingSomewhere", false);
            
            SetContentView(Resource.Layout.Detail);

            ViewPager viewPager = FindViewById<ViewPager>(Resource.Id.viewPagerDetails);
            viewPager.Adapter = new DetailPagerAdapter(this, this.SupportFragmentManager, appointmentRecord, workingHere, workingComplete, workingSomewhere) ;




            Button aanmeldButton = FindViewById<Button>(Resource.Id.aanmeldButton);
            if (workingSomewhere)
                aanmeldButton.Enabled = false;

            if (workingHere)
            {
                aanmeldButton.Text = Resources.GetString(Resource.String.StopWorking);
                if (!workingComplete)
                    aanmeldButton.Enabled = false;
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

        public void OnTasksComplete(object sender, bool complete)
        {
            workingComplete = true;
            if (complete)
                Toast.MakeText(this, "tasks complete" + complete.ToString(), ToastLength.Long).Show();

            FindViewById<Button>(Resource.Id.aanmeldButton).Enabled = complete;
        }


        //protected void OnListItemClick(Object sender,
        //                            Android.Widget.AdapterView.ItemClickEventArgs e)
        //{
        //    if (workingHere)
        //    {
        //        ListView listView = sender as ListView;
        //        var itemView = listView.GetChildAt(e.Position - listView.FirstVisiblePosition);

        //        CheckBox taskCheck = itemView.FindViewById<CheckBox>(Resource.Id.checkBox1);
        //        taskCheck.Enabled = true;
        //        taskCheck.Toggle();
        //    }
        //    else if (workingSomewhere)
        //    {
        //        Toast.MakeText(this, Resources.GetString(Resource.String.NotWorkingHereError), ToastLength.Long).Show();
        //    }
        //    else
        //    {
        //        Toast.MakeText(this, Resources.GetString(Resource.String.NotWorkingError), ToastLength.Long).Show();
        //    }
        //}

        protected void toggleAanmeldButton(Button aanmeldButton)
        {
            Resources res = Resources;
            _database db = new _database(this);
            // aanmeldbutton
            if (aanmeldButton.Text == res.GetString(Resource.String.StartWorking))
            {
                // send time to to api (maybe in updateTimeForAppointment?)
                db.updateTimeForAppointment(int.Parse(appointmentId), "time_start");
                workingHere = true;
                aanmeldButton.Text = res.GetString(Resource.String.StopWorking);
                aanmeldButton.Enabled = false;
                //for (int i = 0; i < listView.ChildCount; i++)
                //{
                //    var itemView = listView.GetChildAt(i);
                //    CheckBox cb = (CheckBox)itemView.FindViewById<CheckBox>(Resource.Id.checkBox1);
                //    cb.Enabled = true;
                //}

            }
            // afmeldbutton hit
            else
            {
                db.updateTimeForAppointment(int.Parse(appointmentId), "time_finish");
                // send time to to api (maybe in updateTimeForAppointment?)
                // TODO
                //

                aanmeldButton.Enabled = false;
                Toast.MakeText(this, "Appointment finished!", ToastLength.Long).Show();
                // go back
                Finish();
            }
        }
    }

}