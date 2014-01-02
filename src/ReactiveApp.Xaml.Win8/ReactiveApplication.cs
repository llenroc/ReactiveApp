using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

using ReactiveApp.Interfaces;
using ReactiveApp.Services;
using ReactiveApp.Xaml.Controls;
using ReactiveApp.Xaml.Services;
using ReactiveUI;
using Splat;

#if !WINDOWS_PHONE
using Windows.UI.Xaml;
using Windows.ApplicationModel.Activation;
#else
using System.Windows;
using Microsoft.Phone.Shell;
using System.Reactive.Linq;
#endif

namespace ReactiveApp.Xaml
{
    public abstract class ReactiveApplication : Application
    {
#if !WINDOWS_PHONE
        // an app can only launch once and we want to remember that value
        internal readonly ISubject<LaunchActivatedEventArgs> launched = new ReplaySubject<LaunchActivatedEventArgs>();
#else
        internal readonly IPhoneFrameHelper frameHelper;
#endif

        public ReactiveApplication()
        {
            this.Log().Info("Starting ReactiveApplication.");
            this.Log().Info("Creating Dependency Resolver.");
            var resolver = this.CreateDependencyResolver();
            this.Log().Info("Initialize Dependency Resolver.");
            resolver.InitializeSplat();
            resolver.InitializeReactiveUI();
            Locator.Current = resolver;

            this.Log().Info("Creating Shell.");
            this.Shell = this.CreateShell();
#if WINDOWS_PHONE
            this.frameHelper = new PhoneFrameHelper(this.Shell);
#endif

            this.Log().Info("Creating SuspensionService.");
#if !WINDOWS_PHONE
            var suspensionService = new WinRTSuspensionService(this, launched);
#else
            this.ApplicationLifetimeObjects.Add(new PhoneApplicationService());
            var suspensionService = new WP8SuspensionService(this, this.frameHelper);
#endif
            this.SuspensionService = suspensionService;            

            this.Log().Info("Register services.");
            this.Configure();
        }

        protected abstract void Configure();

        protected abstract IMutableDependencyResolver CreateDependencyResolver();

        protected abstract ReactiveShell CreateShell();

        public abstract IObservable<Unit> View(string args);
        
        protected virtual IObservable<Unit> Activate()
        {
            this.Log().Info("Activating Shell.");
#if !WINDOWS_PHONE
            if (Window.Current.Content != this)
            {
                Window.Current.Content = this;
            }
            Window.Current.Activate();
            return Observable.Return<Unit>(Unit.Default);
#else
            return this.frameHelper.Activate();
#endif
        }

        /// <summary>
        /// The designer does not seem to like interfaces so we just implement this method directly instead of via extension methods on IEnableLogger.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">ILogManager is null. This should never happen, your dependency resolver is broken</exception>
        private IFullLogger Log()
        {
            var factory = Locator.Current.GetService<ILogManager>();
            if (factory == null)
            {
                throw new Exception("ILogManager is null. This should never happen, your dependency resolver is broken");
            }

            return factory.GetLogger<ReactiveApplication>();
        }

#if !WINDOWS_PHONE
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            this.launched.OnNext(args);
        }
#endif

#if !WINDOWS_PHONE
        public void Close()
        {
            this.Exit();
        }
#else
        public void Close()
        {
            this.Terminate();
        }
#endif

        /// <summary>
        /// Gets the shell.
        /// </summary>
        /// <value>
        /// The shell.
        /// </value>
        public ReactiveShell Shell { get; private set; }

        /// <summary>
        /// Gets the suspension service.
        /// </summary>
        /// <value>
        /// The suspension service.
        /// </value>
        public ISuspensionService SuspensionService { get; private set; }
    }
}
