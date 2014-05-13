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

namespace TestApp.Android
{
    [Application]
    class App : Application
    {
        public override void OnCreate()
        {
            base.OnCreate();

            var bootstrapper = new Bootstrapper(this);
            bootstrapper.Run();
        }
    }
}