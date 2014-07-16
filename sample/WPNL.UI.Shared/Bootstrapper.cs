using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using ReactiveApp;
using ReactiveApp.App;
using ReactiveApp.Xaml;
using ReactiveUI;
using Splat;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using WPNL.Core;
using WPNL.Core.ViewModels;
using WPNL.UI.Views;

namespace WPNL.UI
{
    public class Bootstrapper : WinRTBootstrapper
    {
        public Bootstrapper(Frame frame, AutoSuspendHelper suspendHelper)
            : base(frame, suspendHelper)
        { }

        protected override IReactiveApplication CreateApplication()
        {
            return new WPNLApp();
        }

        protected override void AfterBootstrapping()
        {
            base.AfterBootstrapping();

            Locator.CurrentMutable.RegisterView<MainView, MainViewModel>();
        }
    }
}
