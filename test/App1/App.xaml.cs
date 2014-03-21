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
using Splat;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace App1
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

            this.SetupStartup();
            this.SetupErrorHandling();
        }

        protected override void Configure()
        {
        }


        /// <summary>
        /// Creates the dependency resolver based on Munq IoC.
        /// </summary>
        /// <returns></returns>
        protected override IMutableDependencyResolver CreateDependencyResolver()
        {
            this.container = new IocContainer();
            return new MunqDependencyResolver(this.container);
        }


        /// <summary>
        /// Creates the shell.
        /// </summary>
        /// <returns></returns>
        protected override ReactiveShell CreateShell()
        {
            ReactiveShell shell = new ReactiveShell();
            // Set the default language
            shell.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
            return shell;
        }


        /// <summary>
        /// Called when an app in launched or activated with the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public override IObservable<Unit> View(string args)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            return this.Shell.NavigateAsync(typeof(MainView), args).SelectMany(this.Activate());
        }
    }
}
