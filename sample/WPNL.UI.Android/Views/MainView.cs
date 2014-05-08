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
using ReactiveApp.Android.Views;
using ReactiveUI;
using WPNL.Core.ViewModels;

namespace WPNL.UI.Android.Views
{
    [Activity(Label = "MainView", MainLauncher = true)]
    public class MainView : AndroidReactiveView, IViewFor<MainViewModel>
    {
        public MainViewModel ViewModel
        {
            get;
            set;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here

            this.SetContentView(Resource.Layout.MainView);
        }
    }
}