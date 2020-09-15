using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using ZAPP.Records;
using static Android.Widget.CompoundButton;

namespace ZAPP.Adapters
{
    [Activity(Label = "TaskListViewAdapter")]
    public class TaskListViewAdapter : BaseAdapter<TaskRecord>
    {
        List<TaskRecord> items;
        Activity context;
        public event EventHandler<bool> TasksComplete;
        public string appointmentId;

        public TaskListViewAdapter(Activity context, List<TaskRecord> items, string appointmentId ) : base()
        {
            this.context = context;
            this.items = items;
            this.appointmentId = appointmentId;
        }
        public override TaskRecord this[int position]
        {
            get { return items[position]; }
        }

        public override int Count
        {
            get { return items.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //Console.WriteLine($"Position: {position.ToString()}, converView: {convertView}, parent: {parent}");
            var item = items[position];
            View view = convertView;

            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.taskListRow, null);
            }

            view.FindViewById<TextView>(Resource.Id.taskText).Text = (position+1).ToString().PadLeft(2, '0') + " - " + item.description;
            CheckBox taskCheck = (CheckBox)view.FindViewById(Resource.Id.checkBox1);

            if (Singleton.currentlyWorking == appointmentId)
                taskCheck.Enabled = true;

            taskCheck.Tag = item.id + ";" + position.ToString();


            taskCheck.Checked = item.complete;
            taskCheck.CheckedChange += (sender, e) =>
            {
                bool check = e.IsChecked;
                CheckBox chk = (CheckBox)sender;
                String[] tags = chk.Tag.ToString().Split(";");
                int id = int.Parse(tags[0]);

                _database db = new _database(context);

                items[int.Parse(tags[1])].complete = check;
                db.toggleTaskCompleteness(id, check);
                checkCompleteness();
            };

            return view;
        }

        public void checkCompleteness()
        {
            bool complete = true;
            foreach (var item in items)
            {
                if (!item.complete)
                {
                    TasksComplete?.Invoke(this, false);
                    return;
                }
            }
            TasksComplete?.Invoke(this, complete);
        }
    }
}