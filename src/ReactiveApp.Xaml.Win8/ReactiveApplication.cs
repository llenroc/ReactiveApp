using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

using ReactiveApp.Interfaces;
using ReactiveApp.Xaml.Controls;
using ReactiveApp.Xaml.Services;
using ReactiveUI;
using ReactiveUI.Mobile;

#if !WINDOWS_PHONE
using Windows.UI.Xaml;
using Windows.ApplicationModel.Activation;
using System.Reactive;
using ReactiveApp.Services;
#else
using System.Windows;
#endif

namespace ReactiveApp.Xaml
{
    public abstract class ReactiveApplication<T, U> : Application, IEnableLogger
        where T : class, IShell<T, U>
        where U : class, IView<T, U>
    {
#if !WINDOWS_PHONE
        internal readonly ReplaySubject<LaunchActivatedEventArgs> launched = new ReplaySubject<LaunchActivatedEventArgs>();
#endif

        public ReactiveApplication()
        {
            this.Log().Info("Starting ReactiveApplication.");
            this.Log().Info("Creating SuspensionService.");
#if !WINDOWS_PHONE
            var suspensionService = new WinRTSuspensionService(this, launched);
#else
            var suspensionService = new WP8SuspensionService(this);
#endif
            this.SuspensionService = suspensionService;

            this.Log().Info("Creating Shell.");
            this.Shell = this.CreateShell();

            this.Log().Info("Creating Dependency Resolver.");
            var resolver = this.CreateDependencyResolver();
            this.Log().Info("Initialize Dependency Resolver.");
            resolver.InitializeResolver();
            RxApp.DependencyResolver = resolver;

            this.Configure();
        }

        protected abstract void Configure();

        protected abstract IMutableDependencyResolver CreateDependencyResolver();

        protected abstract IShell<T, U> CreateShell();

        public abstract IObservable<Unit> View(string args);

        protected virtual IObservable<Unit> Activate()
        {
            this.Log().Info("Activating Shell.");
            return this.Shell.Activate();
        }

#if !WINDOWS_PHONE
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            this.launched.OnNext(args);
        }
#endif

        /// <summary>
        /// The shell used for navigation.
        /// </summary>
        public IShell<T, U> Shell { get; private set; }

        public ISuspensionService SuspensionService { get; private set; }
    }
}
