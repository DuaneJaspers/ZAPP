using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using ZAPP.Records;

namespace ZAPP
{
    [Activity(Label = "TaskListViewAdapter")]
    public class TaskListViewAdapter : BaseAdapter<TaskRecord>
    {
        List<TaskRecord> items;
        Activity context;
        bool workingHere;
        public event EventHandler<bool> TasksComplete;

        public TaskListViewAdapter(Activity context, List<TaskRecord> items, bool workingHere ) : base()
        {
            this.context = context;
            this.items = items;
            this.workingHere = workingHere;
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
            Console.WriteLine($"Position: {position.ToString()}, converView: {convertView}, parent: {parent}");
            var item = items[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.taskListRow, null);
            }
            view.FindViewById<TextView>(Resource.Id.taskText).Text = (position+1).ToString().PadLeft(2, '0') + " - " + item.description;
            CheckBox taskCheck = (CheckBox)view.FindViewById(Resource.Id.checkBox1);
            if (workingHere)
            {
                taskCheck.Enabled = true;
            }
            taskCheck.Tag = item.id;
            //taskCheck.SetOnCheckedChangeListener(null);
            
            taskCheck.Checked = item.complete;
            //taskCheck.SetOnCheckedChangeListener(new CheckedChangeListener(this.context));
            taskCheck.CheckedChange += (sender, e) =>
            {
                bool check = e.IsChecked;
                CheckBox chk = (CheckBox)sender;
                int id = (int)chk.Tag;
                //Console.WriteLine(id.ToString() + "is the id of the string ");
                _database db = new _database(context);
                item.complete = check;
                if (check)
                {
                    db.toggleTaskCompleteness(id, true);
                }
                else
                {
                    db.toggleTaskCompleteness(id, false);
                }
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