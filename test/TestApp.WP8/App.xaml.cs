using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Resources;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Munq;
using ReactiveApp;
using ReactiveApp.Services;
using ReactiveApp.Xaml;
using ReactiveApp.Xaml.Controls;
using ReactiveUI;
using Splat;
using TestApp.WP8.Resources;

namespace TestApp.WP8
{
    public partial class App : ReactiveApplication
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
#if !WINDOWS_PHONE
            shell.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
#else
            shell.Language = XmlLanguage.GetLanguage("en-US");
#endif
            shell.FlowDirection = FlowDirection.LeftToRight;
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
                
#if !WINDOWS_PHONE
                this.DebugSettings.EnableFrameRateCounter = true;
#else
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode,
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Prevent the screen from turning off while under the debugger by disabling
                // the application's idle detection.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
#endif
            }
#endif
            return this.Shell.NavigateAsync(typeof(MainView), args).SelectMany(this.Activate());
        }
    }
}