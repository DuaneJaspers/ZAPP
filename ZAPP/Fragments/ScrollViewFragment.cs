using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using ZAPP.Records;

namespace ZAPP.Fragments
{
    public class ScrollViewFragment : Android.Support.V4.App.Fragment
    {
        public string appointment_id => Arguments.GetString("appointment_id");
        public string address => Arguments.GetString("address");
        public string zipcode => Arguments.GetString("zipcode");
        public string phonenumber => Arguments.GetString("phonenumber");
        public string comment => Arguments.GetString("comment");
        public string tab => Arguments.GetString("tab");


        public static ScrollViewFragment NewInstance(AppointmentRecord appointmentRecord, string tab)
        {
            var bundle = new Bundle();
            bundle.PutString("appointment_id", appointmentRecord.id.ToString());
            bundle.PutString("address", appointmentRecord.client_address);
            bundle.PutString("zipcode", appointmentRecord.client_zipcode);
            bundle.PutString("phonenumber", appointmentRecord.client_phonenumber);
            bundle.PutString("comment", appointmentRecord.comment);
            bundle.PutString("tab", tab);


            return new ScrollViewFragment { Arguments = bundle };
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                return null;
            }

            var textView = new TextView(Activity);
            var padding = Convert.ToInt32(TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, Activity.Resources.DisplayMetrics));
            textView.SetPadding(padding, padding, padding, padding);
            textView.TextSize = 56;
            if (tab == "address")
            {
                string addressTab = Resources.GetString(Resource.String.addressTab);
                addressTab = String.Format(addressTab, address, zipcode, phonenumber);
                textView.Text = addressTab;
            } else
            {
                textView.Text = comment;
            }

            var scroller = new ScrollView(Activity);
            scroller.AddView(textView);

            return scroller;
        }
    }
}