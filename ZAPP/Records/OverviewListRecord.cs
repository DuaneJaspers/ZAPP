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
using Mono.Data.Sqlite;

namespace ZAPP.Records

{
    class OverviewListRecord
    {
        public int id;
        public string name;
        public string address;
        public string phonenumber;
        public string zipcode;
        public string city;
        public string datetime;
        public bool working;

        public OverviewListRecord(int id, string name, string address, string phonenumber, string zipcode, string city, string datetime, string start_time)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.phonenumber = phonenumber;
            this.zipcode = zipcode;
            this.city = city;
            this.datetime = datetime;
            if (String.IsNullOrEmpty(start_time))
                this.working = false;
            else
                this.working = true;
            
        }
    }
}