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

namespace WPNL.UI.Android
{
    [Application]
    public class App : Application
    {
        private Bootstrapper bootstrapper;
        public App(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {

        }

        public override void OnCreate()
        {
            base.OnCreate();

            bootstrapper = new Bootstrapper(this);
            bootstrapper.Run();
        }
    }
}