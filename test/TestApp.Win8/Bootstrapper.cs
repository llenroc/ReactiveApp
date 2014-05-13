using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using ReactiveApp;
using ReactiveApp.App;
using ReactiveApp.Xaml;
using Splat;
using TestApp.ViewModels;
using TestApp.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;

namespace TestApp
{
    public class Bootstrapper : WinRTBootstrapper
    {
        private Munq.IocContainer ioc;

        public Bootstrapper(Frame frame, ISubject<LaunchActivatedEventArgs> launched)
            : base(frame, launched)
        {
            ioc = new Munq.IocContainer();
        }

        protected override IReactiveApplication CreateApplication()
        {
            return new TestApp();
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
    }
}
