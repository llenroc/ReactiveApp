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
using ReactiveApp;
using ReactiveApp.Android;
using ReactiveApp.App;
using ReactiveApp.Services;
using Splat;
using WPNL.UI.Android.Views;
using WPNL.ViewModels;

namespace WPNL.UI.Android
{
    public class Bootstrapper : AndroidBootstrapper
    {
        private Munq.IocContainer ioc;

        public Bootstrapper(Application application)
            : base(application)
        {
            this.ioc = new Munq.IocContainer();
        }

        /// <summary>
        /// Creates the application.
        /// </summary>
        /// <returns></returns>
        protected override IReactiveApplication CreateApplication()
        {
            return new WPNLApp();
        }

        protected override void AfterBootstrapping()
        {
            base.AfterBootstrapping();

            Locator.CurrentMutable.RegisterView<MainView, MainViewModel>();
        }

        protected override IDependencyResolver CreateDependencyResolver()
        {
            return new MunqDependencyResolver(ioc);
        }

        protected override ISerializer CreateSerializer()
        {
            return new WPNLJsonSerializer();
        }
    }
}