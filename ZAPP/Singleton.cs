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
    class Singleton
    {
        private static readonly Singleton SingletonInstance = new Singleton();
        public static string currentlyWorking { set; get; }
        public static bool tasksComplete { set; get; }

        public static string userToken { set; get; }

        static Singleton()
        {
        }

        public static Singleton getObject
        {
            get 
            {
                return SingletonInstance;
            }
        }


    }
}