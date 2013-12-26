using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ReactiveApp.Interfaces;
using ReactiveApp.Xaml;
using ReactiveApp.Xaml.Controls;
using ReactiveUI;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace App1
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : ReactiveApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        protected override void Configure()
        {
            throw new NotImplementedException();
        }

        protected override IMutableDependencyResolver CreateDependencyResolver()
        {
            throw new NotImplementedException();
        }

        protected override ReactiveShell CreateShell()
        {
            throw new NotImplementedException();
        }

        public override IObservable<System.Reactive.Unit> View(string args)
        {
            throw new NotImplementedException();
        }
    }
}
