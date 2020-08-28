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
 

            return view;
        }

    }
}