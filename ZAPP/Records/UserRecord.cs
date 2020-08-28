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
    class UserRecord
    {
        public int id;
        public string username;
        public string password;

        public UserRecord(SqliteDataReader record)
        {
            this.id = (int)(Int64)record["id"];
            this.username = (string)record["code"];
            this.password = (string)record["description"];
        }
    }
}