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
        bool workingHere = false;
        bool workingSomewhere = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            this.appointmentId = Intent.GetStringExtra("ID");
            this.workingHere = Intent.GetBooleanExtra("working", false);
            // Check if working somewhere else to prevent working at two places
            if (!workingHere)
                workingSomewhere = Intent.GetBooleanExtra("workingSomewhere", false);
            
            _database db = new _database(this);
            tasks = db.getAllTasksByAppointmentId(appointmentId);
            records = new List<TaskRecord>();
            foreach (TaskRecord taskRecord in tasks)
            {
                records.Add(taskRecord);
            }

            SetContentView(Resource.Layout.Detail);
            listView = FindViewById<ListView>(Resource.Id.taskList);


            var tasklistAdapter = new TaskListViewAdapter(this, records, workingHere);
            tasklistAdapter.TasksComplete += OnTasksComplete;
           

            listView.Adapter = tasklistAdapter;
            listView.ItemClick += OnListItemClick;
            Button aanmeldButton = FindViewById<Button>(Resource.Id.aanmeldButton);
            if (workingSomewhere)
                aanmeldButton.Enabled = false;

            if (workingHere)
                aanmeldButton.Text = Resources.GetString(Resource.String.StopWorking);

            aanmeldButton.Click += delegate
            {
                toggleAanmeldButton(aanmeldButton);
            };

        }

        public void OnTasksComplete(object sender, bool complete)
        {
            Toast.MakeText(this, "tasks complete" + complete.ToString(), ToastLength.Long).Show();
            FindViewById<Button>(Resource.Id.aanmeldButton).Enabled = complete;
        }
        

        protected void OnListItemClick(Object sender,
                                    Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            if (workingHere)
            {
                ListView listView = sender as ListView;
                var itemView = listView.GetChildAt(e.Position - listView.FirstVisiblePosition);

                CheckBox taskCheck = itemView.FindViewById<CheckBox>(Resource.Id.checkBox1);
                taskCheck.Enabled = true;
                taskCheck.Toggle(); 
            }
        }

        protected void toggleAanmeldButton(Button aanmeldButton)
        {
            Resources res = Resources;
            _database db = new _database(this);
            if (aanmeldButton.Text == res.GetString(Resource.String.StartWorking))
            {
                // send time to to api (maybe in updateTimeForAppointment?)
                db.updateTimeForAppointment(int.Parse(appointmentId), "time_start");
                workingHere = true;
                aanmeldButton.Text = res.GetString(Resource.String.StopWorking);
                aanmeldButton.Enabled = false;

            }
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