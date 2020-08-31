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

namespace ZAPP
{
    [Activity(Label = "DetailActivity")]
    public class DetailActivity : Activity
    {
        ListView listView;
        List<TaskRecord> records;
        ArrayList tasks;
        string appointmentId;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.appointmentId = Intent.GetStringExtra("ID");
            bool working = Intent.GetBooleanExtra("working", false);
            bool workingSomewhere = false;
            if (!working)
            {

                workingSomewhere = Intent.GetBooleanExtra("workingSomewhere", false);
            }
            
            _database db = new _database(this);
            tasks = db.getAllTasksByAppointmentId(appointmentId);
            records = new List<TaskRecord>();
            foreach (TaskRecord taskRecord in tasks)
            {
                records.Add(taskRecord);
            }

            SetContentView(Resource.Layout.Detail);
            listView = FindViewById<ListView>(Resource.Id.taskList);
   
            
            listView.Adapter = new TaskListViewAdapter(this, records, working);
            listView.ItemClick += OnListItemClick;

            Button aanmeldButton = FindViewById<Button>(Resource.Id.aanmeldButton);
            if (working || workingSomewhere)
            {
                aanmeldButton.Enabled = false;
            }
            aanmeldButton.Click += delegate
            {
                toggleAanmeldButton(aanmeldButton);
            };

        }
        

        protected void OnListItemClick(Object sender,
                                    Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            ListView listView = sender as ListView;
            var itemView = listView.GetChildAt(e.Position);

            CheckBox taskCheck = itemView.FindViewById<CheckBox>(Resource.Id.checkBox1);
            taskCheck.Toggle();
        }

        protected void toggleAanmeldButton(Button aanmeldButton)
        {
            Console.WriteLine("Button CLICKED");
            Resources res = Resources;
            _database db = new _database(this);
            if (aanmeldButton.Text == res.GetString(Resource.String.StartWorking))
            {
                // send time to to api (maybe in updateTimeForAppointment?)
                db.updateTimeForAppointment(int.Parse(appointmentId), "time_start");
                aanmeldButton.Text = res.GetString(Resource.String.StopWorking);
                aanmeldButton.Enabled = false;
            }
            else
            {
                db.updateTimeForAppointment(int.Parse(appointmentId), "time_finish");
                aanmeldButton.Text = "NEW CLICK WOOHOO";
                aanmeldButton.Enabled = false;
                // go back
            }
        }
    }

}