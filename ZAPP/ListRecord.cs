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
    class ListRecord
    {
        public string id;
        public string code;
        public string description;

        public ListRecord(string id, string code, string description)
        {
            this.id = id;
            this.code = code;
            this.description = description;
        }
    }
}