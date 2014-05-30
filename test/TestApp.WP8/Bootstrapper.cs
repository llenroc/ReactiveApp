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
using TestApp.BindingTypeConverters;
using TestApp.ViewModels;

namespace TestApp
{
    public class Bootstrapper : PhoneBootstrapper
    {
        private Munq.IocContainer ioc;

        public Bootstrapper(PhoneApplicationFrame frame, IArgumentsProvider arguments)
            :base(frame, arguments)
        {
            this.ioc = new Munq.IocContainer();
        }

        protected override IReactiveApplication CreateApplication()
        {
            return new TestApp();
        }

        protected override INavigationSerializer CreateNavigationSerializer()
        {
            return base.CreateNavigationSerializer();
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
