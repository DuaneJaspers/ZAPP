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
using ZAPP.Records;

namespace ZAPP
{
    public class AddressFragment : Android.Support.V4.App.Fragment
    {
        public string appointment_id => Arguments.GetString("appointment_id");
        public string address => Arguments.GetString("address");
        public string zipcode => Arguments.GetString("zipcxode");
        public string phonenumber => Arguments.GetString("phonenumber");

        public static AddressFragment NewInstance(AppointmentRecord appointmentRecord)
        {
            var bundle = new Bundle();
            bundle.PutString("appointment_id", appointmentRecord.id.ToString());
            bundle.PutString("address", appointmentRecord.client_address);
            bundle.PutString("zipcode", appointmentRecord.client_zipcode);
            bundle.PutString("phonenumber", appointmentRecord.client_phonenumber);
            return new AddressFragment { Arguments = bundle };
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
            textView.TextSize = 24;
            textView.Text = $"Address: {address} {zipcode} \n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"\n" +
                $"Telefoon :{phonenumber}";


            var scroller = new ScrollView(Activity);
            scroller.AddView(textView);

            return scroller;
        }
    }
}