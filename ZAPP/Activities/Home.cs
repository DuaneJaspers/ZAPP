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
using ZAPP.Adapters;


namespace ZAPP.Activities
{
    [Activity(Label = "HomeActivity")]
    class Home : Activity
    {
        ListView listView;
        List<OverviewListRecord> records;
        ArrayList result;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            syncDatabase();
        }

        private void fillData()
        {
            _database db = new _database(this);
            //db.syncDatabase();
            result = db.getAllAppointments();
            records = new List<OverviewListRecord>();
            foreach (AppointmentRecord appointment in result)
            {
                if (String.IsNullOrEmpty(appointment.time_finish))
                {
                    OverviewListRecord row = new OverviewListRecord(appointment.id,
                                                    appointment.client_name,
                                                    appointment.client_address,
                                                    appointment.client_phonenumber,
                                                    appointment.client_zipcode,
                                                    appointment.client_city,
                                                    appointment.datetime,
                                                    appointment.time_start);
                    if (!String.IsNullOrEmpty(appointment.time_start))
                        Singleton.currentlyWorking = appointment.id.ToString();
                    records.Add(row);
                }
                
            }
            SetContentView(Resource.Layout.overview);
            Android.Support.V4.Widget.SwipeRefreshLayout pullToRefresh = FindViewById<Android.Support.V4.Widget.SwipeRefreshLayout>(Resource.Id.pullToRefresh);
            pullToRefresh.Refresh += delegate (object sender, System.EventArgs e)
            {
                syncDatabase();
            };
            listView = FindViewById<ListView>(Resource.Id.Overview);
            listView.Adapter = new HomeListViewAdapter(this, records);
            listView.ItemClick += OnListItemClick;
        }

        protected void OnListItemClick(object sender,
                                    Android.Widget.AdapterView.ItemClickEventArgs e)
        {
            var t = records[e.Position];
            
            var intent = new Intent(this, typeof(DetailActivity));
            intent.PutExtra("ID", t.id.ToString());
            intent.PutExtra("working", t.working);
            intent.PutExtra("description", t.address.ToString());
            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            Console.WriteLine("activity resumed");
            fillData();
            base.OnActivityResult(requestCode, resultCode, data);
        }

        private void syncDatabase()
        {
            _database db = new _database(this);
            db.syncDatabase();
            fillData();
        }
    }
}