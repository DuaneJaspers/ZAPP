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
using ZAPP.Records;

namespace ZAPP
{
    [Activity(Label = "TaskListViewAdapter")]
    public class TaskListViewAdapter : BaseAdapter<TaskRecord>
    {
        List<TaskRecord> items;
        Activity context;

        public TaskListViewAdapter(Activity context, List<TaskRecord> items) : base()
        {
            this.context = context;
            this.items = items;
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
            var item = items[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.taskListRow, null);
            }
            view.FindViewById<TextView>(Resource.Id.taskText).Text = item.description;
            CheckBox taskCheck = (CheckBox)view.FindViewById(Resource.Id.checkBox1);
            taskCheck.SetOnCheckedChangeListener(null);
            taskCheck.Checked = item.complete;
            taskCheck.SetOnCheckedChangeListener(new CheckedChangeListener(this.context));
            return view;
        }


        // code from http://martynnw.blogspot.com/2014/10/xamarin-android-listviews-checkboxes-on.html
        private class CheckedChangeListener : Java.Lang.Object, CompoundButton.IOnCheckedChangeListener
        {
            private Activity activity;

            public CheckedChangeListener(Activity activity)
            {
                this.activity = activity;
            }
            
            public void OnCheckedChanged(CompoundButton buttonView, bool isChecked)
            {
                if (isChecked)
                {
                    string name = "check";
                    Console.WriteLine(name);
                } else
                {
                    Console.WriteLine("unchecked");
                }
            }
        }
    }
}