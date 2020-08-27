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
using Mono.Data.Sqlite;

namespace ZAPP.Records
{
    public class TaskRecord
    {
        public int? id;
        public int appointment_id;
        public string description;
        public bool complete;

        public TaskRecord(JsonValue record, int appointment_id)
        {
            this.appointment_id = appointment_id;
            this.description = (string)record["description"];
        }

        public TaskRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.appointment_id = (int)(Int64)record["appointment_id"];
            this.description = (string)record["description"];
            this.complete = (bool)record["complete"];
        }
    }
}