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

namespace ZAPP
{
    class DetailPagerAdapter : Android.Support.V4.App.FragmentPagerAdapter
    {
        Context context;
        int[] titles =
        {
           Resource.String.Tasks,
           Resource.String.Address
        };
        int[] layouts = {Resource.Layout.tasks, Resource.Layout.address};
        AppointmentRecord appointmentRecord;
        Android.Support.V4.App.FragmentManager fragmentManager;
        bool workingHere = false;
        bool workingSomewhere = false;
        bool workingComplete = false;


        public DetailPagerAdapter(Activity context,  Android.Support.V4.App.FragmentManager fragmentManager, AppointmentRecord appointmentRecord, bool workingHere, bool workingComplete, bool workingSomewhere): base(fragmentManager)
        {
            this.fragmentManager = fragmentManager;
            this.context = context;
            this.appointmentRecord = appointmentRecord;
            this.workingHere = workingHere;
            this.workingComplete = workingComplete;
            this.workingSomewhere = workingSomewhere;

        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(context.Resources.GetString(titles[position]));
        }
    

        public override Android.Support.V4.App.Fragment GetItem(int position)
        {

            Bundle args = new Bundle();
            args.PutBoolean("workingHere", workingHere);
            args.PutBoolean("workingComplete", workingComplete);
            args.PutBoolean("workingSomewhere", workingSomewhere);

            args.PutString("ID", appointmentRecord.id.ToString());
            if (position == 1)
            {
                return (Android.Support.V4.App.Fragment)
                    AddressFragment.NewInstance(appointmentRecord);
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
                return layouts.Count();
            }
        }

    }
}