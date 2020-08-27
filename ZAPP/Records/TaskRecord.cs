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
    class TaskRecord
    {
        public int id;
        public DateTime datetime;
        public string client_name;
        public string client_address;
        public string client_zipcode;
        public string client_city;
        public string client_phonenumber;
        public Time time_start;
        public Time time_finish;
        public string comment;


        public TaskRecord(JsonValue record)
        {
            this.id = (int)(Int64)record["id"];
            this.datetime = (DateTime)record["datetime"];
            this.client_name = (string)record["client_name"];
            this.client_address = (string)record["client_address"];
            this.client_zipcode = (string)record["client_zipcode"];
            this.client_city = (string)record["client_city"];
            this.client_phonenumber = (string)record["client_phonenumber"];
            this.comment = (string)record["comment"];
        }

        public TaskRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.datetime = (DateTime)record["datetime"];
            this.client_name = (string)record["client_name"];
            this.client_address = (string)record["client_address"];
            this.client_zipcode = (string)record["client_zipcode"];
            this.client_city = (string)record["client_city"];
            this.client_phonenumber = (string)record["client_phonenumber"];
            this.time_start = (Time)record["time_start"];
            this.time_finish = (Time)record["time_finish"];
            this.comment = (string)record["comment"];
        }
    }
}