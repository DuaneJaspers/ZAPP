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

namespace ZAPP
{
    public static class Global
    {

        public static SqliteConnection sqliteConnection;

        public static void saveConnection(SqliteConnection connection)
        {
            sqliteConnection = connection;
            sqliteConnection.Open();
        }
    }
}