using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Service.Autofill;
using Android.Service.QuickSettings;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;
using ZAPP.Records;
using ZAPP.Fragments;


namespace ZAPP.Adapters
{
    class DetailViewPagerAdapter : Android.Support.V4.App.FragmentPagerAdapter
    {
        Context context;
        int[] titles =
        {
           Resource.String.Tasks,
           Resource.String.Address,
           Resource.String.Comment
        };
        AppointmentRecord appointmentRecord;
        Android.Support.V4.App.FragmentManager fragmentManager;

        public DetailViewPagerAdapter(Activity context,  Android.Support.V4.App.FragmentManager fragmentManager, AppointmentRecord appointmentRecord): base(fragmentManager)
        {
            this.fragmentManager = fragmentManager;
            this.context = context;
            this.appointmentRecord = appointmentRecord;

        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(context.Resources.GetString(titles[position]));
        }
    

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {

            Bundle args = new Bundle();

            args.PutString("ID", appointmentRecord.id.ToString());
            if (position == 2)
            {
                return (Android.Support.V4.App.Fragment)
                    ScrollViewFragment.NewInstance(appointmentRecord, "comment");
            }
            else if (position == 1)
            {
                return (Android.Support.V4.App.Fragment)
                    ScrollViewFragment.NewInstance(appointmentRecord, "address");
            }
            else
            {
                Android.Support.V4.App.Fragment taskFragment = new TasksFragment();
                
                taskFragment.Arguments = args;
                return taskFragment;
            }

        }

        //Fill in cound here, currently 0
        public override int Count
        {
            get
            {
                return titles.Count();
            }
        }

    }
}