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
    public class WPNLApp : Application
    {
        private Bootstrapper bootstrapper;
        public override void OnCreate()
        {
            base.OnCreate();

            bootstrapper = new Bootstrapper(this);
            bootstrapper.Run();
        }
    }
}