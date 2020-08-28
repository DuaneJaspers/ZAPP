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

namespace ZAPP
{
    [Activity(Label = "DetailActivity")]
    public class DetailActivity : Activity
    {
        ListView listView;
        List<TaskRecord> records;
        ArrayList tasks;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var id = Intent.GetStringExtra("ID");

            _database db = new _database(this);
            tasks = db.getAllTasksByAppointmentId(id);
            records = new List<TaskRecord>();
            foreach (TaskRecord taskRecord in tasks)
            {
                records.Add(taskRecord);
            }

            SetContentView(Resource.Layout.Detail);
            listView = FindViewById<ListView>(Resource.Id.taskList);
            listView.Adapter = new TaskListViewAdapter(this, records);
            listView.ItemClick += OnListItemClick;

            Button aanmeldButton = FindViewById<Button>(Resource.Id.aanmeldButton);
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
            aanmeldButton.Text = "NEW CLICK WOOHOO";
        }
    }

}