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

namespace ZAPP
{
    [Activity(Label = "DetailActivity")]
    public class DetailActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            var id = Intent.GetStringExtra("ID");
            var code = Intent.GetStringExtra("code");
            var description = Intent.GetStringExtra("description");
            Console.WriteLine($"Got ID: {id}");

            SetContentView(Resource.Layout.Detail);
            FindViewById<TextView>(Resource.Id.textViewer1).Text = id;
            FindViewById<TextView>(Resource.Id.textViewer2).Text = code;
            FindViewById<TextView>(Resource.Id.textViewer3).Text = description;

        }
    }

}