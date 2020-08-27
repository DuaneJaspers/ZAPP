﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace ZAPP
{
    [Activity (Label = "HomeListViewAdapter")]
    class HomeListViewAdapter : BaseAdapter<ListRecord>
    {
        List<ListRecord> items;
        Activity context;

        public HomeListViewAdapter(Activity context, List<ListRecord> items) :base()
        {
            this.context = context;
            this.items = items;
        }
        public override ListRecord this[int position]
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
                view = context.LayoutInflater.Inflate(Resource.Layout.ListRow, null);
            }
            view.FindViewById<TextView>(Resource.Id.Text1).Text = item.name;
            view.FindViewById<TextView>(Resource.Id.Text2).Text = $"{item.adress}, {item.zipcode} {item.city}";
            view.FindViewById<TextView>(Resource.Id.Text3).Text = Convert.ToDateTime(item.datetime).ToString("dd-MM-yyyy");
            view.FindViewById<TextView>(Resource.Id.Text4).Text = Convert.ToDateTime(item.datetime).ToString("HH:mm")+" uur";

            return view;
        }
    }
}