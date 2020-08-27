using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Sql;
using Mono.Data.Sqlite;

namespace ZAPP.Records
{
    public class AppointmentRecord
    {
        public int id;
        public string datetime;
        public string client_name;
        public string client_address;
        public string client_zipcode;
        public string client_city;
        public string client_phonenumber;
        public string time_start;
        public string time_finish;
        public string comment;


        public AppointmentRecord(JsonValue record)
        {
            this.id = (int)(Int64)record["id"];
            this.datetime = (string)record["datetime"];
            this.client_name = (string)@record["client"]["name"];
            this.client_address = (string)@record["client"]["address"];
            this.client_zipcode = (string)@record["client"]["zipcode"];
            this.client_city = (string)@record["client"]["city"];
            this.client_phonenumber = (string)@record["client"]["phonenumber"];
            this.comment = (string)@record["comment"];
        }

        public AppointmentRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.datetime = record["datetime"].ToString();
            this.client_name = (string)record["client_name"];
            this.client_address = (string)record["client_address"];
            this.client_zipcode = (string)record["client_zipcode"];
            this.client_city = (string)record["client_city"];
            this.client_phonenumber = (string)record["client_phonenumber"];
            this.time_start = record["time_start"].ToString();
            this.time_finish = record["time_finish"].ToString();
            this.comment = (string)record["comment"];
        }
    }
}