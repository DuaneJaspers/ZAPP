using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Service.Autofill;
using Android.Service.QuickSettings;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Java.Lang;

namespace ZAPP
{
    class DetailPagerAdapter : PagerAdapter
    {

        Context context;
        string[] titles =
        {
            "Taken", 
            "Address"
        };
        int[] layouts = {Resource.Layout.tasks, Resource.Layout.address};

        public DetailPagerAdapter(Context context)
        {
            this.context = context;
        }


        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            LayoutInflater inflater = LayoutInflater.From(context);
            ViewGroup layout = (ViewGroup)inflater.Inflate(layouts[position], container, false);

            container.AddView(layout);
            return layout;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object @object)
        {
            container.RemoveView((View)@object);
        }

        public override ICharSequence GetPageTitleFormatted(int position)
        {
            return new Java.Lang.String(titles[position].ToString());
        }
    

        public override bool IsViewFromObject(View view, Java.Lang.Object @object)
        {
            return view == @object;
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

    class DetailPagerAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public ListView Tasks { get; set; }
        public ScrollView Address { get; set; }
    }
}