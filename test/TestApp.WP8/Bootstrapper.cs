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
using ReactiveUI.Mobile;
using Splat;
using TestApp.BindingTypeConverters;
using TestApp.ViewModels;
using TestApp.Views;

namespace TestApp
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
            return new TestApp();
        }

        protected override INavigationSerializer CreateNavigationSerializer()
        {
            return new JsonNavigationSerializer();
        }

        protected override void AfterBootstrapping()
        {
            base.AfterBootstrapping();

            Locator.CurrentMutable.Register<IBindingTypeConverter>(() => new ImageSourceBindingConverter());

            Locator.CurrentMutable.RegisterView<MainView, MainViewModel>();
        }

        protected override IDependencyResolver CreateDependencyResolver()
        {
            return new MunqDependencyResolver(ioc);
        }
    }
}
