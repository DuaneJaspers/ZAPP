﻿using System;
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
        public string appointment_id;
        public string description;
        public bool complete;

        public TaskRecord(JsonValue record, string appointment_id)
        {
            this.appointment_id = appointment_id;
            this.description = (string)record["value"];
        }

        public TaskRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.appointment_id = (string)record["appointment_id"];
            this.description = (string)record["description"];
            var temp = record["complete"];
            this.complete = (bool)record["complete"];
        }

        public TaskRecord(int id, string appointment_id, string description, bool complete)
        {
            this.id = id;
            this.appointment_id = appointment_id;
            this.description = description;
            this.complete = complete;
        }

        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            TaskRecord taskRecord = (TaskRecord)obj;
            return (taskRecord.appointment_id == appointment_id && taskRecord.description == description);
        }
    }
}