using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Renderscripts;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ZAPP.Records;
using ZAPP.Adapters;


namespace ZAPP.Fragments
{
    public class TasksFragment : Android.Support.V4.App.ListFragment
    {
        string appointmentId;

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            _database db = new _database(Activity);
            appointmentId = Arguments.GetString("ID");
            var tasks = db.getAllTasksByAppointmentId(appointmentId);
            var records = new List<TaskRecord>();

            foreach (TaskRecord taskRecord in tasks)
            {
                records.Add(taskRecord);
            }

            var tasklistAdapter = new TaskListViewAdapter(Activity, records, appointmentId);
            tasklistAdapter.TasksComplete += OnTasksComplete;


            this.ListAdapter = tasklistAdapter;
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {

            Console.WriteLine($"this items gets clicked {position}");
            if (Singleton.currentlyWorking == appointmentId)
            {
                var itemView = l.GetChildAt(position - l.FirstVisiblePosition);

                CheckBox taskCheck = itemView.FindViewById<CheckBox>(Resource.Id.checkBox1);
                taskCheck.Enabled = true;
                taskCheck.Toggle();
            }
            else if (Singleton.currentlyWorking != null)
            {
                Toast.MakeText(Activity, Resources.GetString(Resource.String.NotWorkingHereError), ToastLength.Long).Show();
            }
            else
            {
                Toast.MakeText(Activity, Resources.GetString(Resource.String.NotWorkingError), ToastLength.Long).Show();
            }
        }
        public void OnTasksComplete(object sender, bool complete)
        {
            Singleton.tasksComplete = complete;
            if (complete)
                Toast.MakeText(Activity, "tasks complete" + complete.ToString(), ToastLength.Long).Show();

            Activity.FindViewById<Button>(Resource.Id.aanmeldButton).Enabled = complete;
        }       
    }
}