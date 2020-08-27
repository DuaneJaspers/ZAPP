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
    [Activity(Label = "HomeActivity")]
    class Home : Activity
    {
        ListView listView;
        List<ListRecord> records;
        ArrayList result;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _database db = new _database(this);
            result = db.getAllTasks();
            records = new List<ListRecord>();
            foreach (TaskRecord value in result)
            {
                ListRecord row = new ListRecord(value.client_name,
                                                value.client_address,
                                                value.client_zipcode,
                                                value.client_city,
                                                value.datetime);
                records.Add(row);
            }

            SetContentView(Resource.Layout.overview);
            listView = FindViewById<ListView>(Resource.Id.Overview);
            listView.Adapter = new HomeListViewAdapter(this, records);
            //listView.ItemClick += OnListItemClick;
        }

        //protected void OnListItemClick(object sender,
        //                            Android.Widget.AdapterView.ItemClickEventArgs e)
        //{
        //    var t = records[e.Position];
        //    var intent = new Intent(this, typeof(DetailActivity));
        //    intent.PutExtra("ID", t.id.ToString());
        //    intent.PutExtra("code", t.code.ToString());
        //    intent.PutExtra("description", t.description.ToString());
        //    StartActivityForResult(intent, 0);
        //}
    }
}