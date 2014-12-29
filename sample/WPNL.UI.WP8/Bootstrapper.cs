using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Phone.Controls;
using ReactiveApp;
using ReactiveApp.App;
using ReactiveApp.Services;
using ReactiveApp.Xaml;
using ReactiveApp.Xaml.Adapters;
using ReactiveUI;
using Splat;
using WPNL.ViewModels;
using WPNL.UI.WP8.Views;

namespace WPNL.UI.WP8
{
    public class Bootstrapper : PhoneBootstrapper
    {
        private Munq.IocContainer ioc;

        public Bootstrapper(PhoneApplicationFrame frame, AutoSuspendHelper suspendHelper)
            :base(frame, suspendHelper)
        {
            this.ioc = new Munq.IocContainer();
        }

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
