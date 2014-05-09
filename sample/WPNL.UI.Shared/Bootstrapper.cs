using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using ReactiveApp.App;
using ReactiveApp.Xaml;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using WPNL.Core;

namespace WPNL.UI
{
    public class Bootstrapper : WinRTBootstrapper
    {
        public Bootstrapper(Frame frame, ISubject<LaunchActivatedEventArgs> launched)
            : base(frame, launched)
        { }

        protected override IReactiveApplication CreateApplication()
        {
            return new WPNLApp();
        }
    }
}
