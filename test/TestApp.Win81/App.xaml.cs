using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Munq;
using ReactiveApp;
using ReactiveApp.Interfaces;
using ReactiveApp.Xaml;
using ReactiveApp.Xaml.Controls;
using ReactiveUI;
using TestApp.Views;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace TestApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : ReactiveApplication
    {
        private IocContainer container;

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
        }

        protected override IMutableDependencyResolver CreateDependencyResolver()
        {
            //this.container = new IocContainer();
            //return new MunqDependencyResolver(this.container);
            throw new NotImplementedException();
        }

        protected override IShell CreateShell()
        {
            throw new NotImplementedException();
        }

        public override IObservable<System.Reactive.Unit> View(string args)
        {
            throw new NotImplementedException();
        }
    }
}