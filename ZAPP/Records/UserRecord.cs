//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Mono.Data.Sqlite;

//namespace ZAPP.Records
//{
//    public class UserRecord
//    {
//        //public int id;
//        //public string username;
//        //public string password;
//        public string userToken;

//        public UserRecord(SqliteDataReader record)
//        {
//            //this.id = (int)(Int64)record["id"];
//            //this.username = (string)record["username"];
//            //this.password = (string)record["password"];
//            this.userToken = (string)record["userToken"];
//            Singleton.userToken = userToken;
//        }
//    }
//}