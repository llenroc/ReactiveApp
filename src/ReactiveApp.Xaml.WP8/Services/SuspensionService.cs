using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Shell;
using ReactiveApp.Services;
using ReactiveApp.Xaml.Controls;
using ReactiveUI;
using ReactiveUI.Mobile;

namespace ReactiveApp.Xaml.Services
{
    /// <summary>
    /// Based on WP8SuspensionHost in ReactiveUI.Mobile
    /// </summary>
    internal class SuspensionService : ISuspensionService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuspensionService"/> class.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="frameHelper">The frame helper.</param>
        public SuspensionService(Application app, IPhoneFrameHelper frameHelper)
        {
            this.IsLaunchingNew =
                Observable.FromEventPattern<LaunchingEventArgs>(
                    x => PhoneApplicationService.Current.Launching += x, x => PhoneApplicationService.Current.Launching -= x)
                    .SelectMany(_ => frameHelper.Arguments.FirstOrDefaultAsync());

            this.IsUnpausing =
                Observable.FromEventPattern<ActivatedEventArgs>(
                    x => PhoneApplicationService.Current.Activated += x, x => PhoneApplicationService.Current.Activated -= x)
                    .Where(x => x.EventArgs.IsApplicationInstancePreserved)
                    .SelectMany(_ => frameHelper.Arguments.FirstOrDefaultAsync());

            // NB: "Applications should not perform resource-intensive tasks 
            // such as loading from isolated storage or a network resource 
            // during the Activated event handler because it increase the time 
            // it takes for the application to resume"
            this.IsResuming =
                Observable.FromEventPattern<ActivatedEventArgs>(
                    x => PhoneApplicationService.Current.Activated += x, x => PhoneApplicationService.Current.Activated -= x)
                    .Where(x => !x.EventArgs.IsApplicationInstancePreserved)
                    .SelectMany(_ => frameHelper.Arguments.FirstOrDefaultAsync());

            // NB: No way to tell OS that we need time to suspend, we have to
            // do it in-process
            this.ShouldPersistState = Observable.Merge(
                Observable.FromEventPattern<DeactivatedEventArgs>(
                    x => PhoneApplicationService.Current.Deactivated += x, x => PhoneApplicationService.Current.Deactivated -= x)
                    .Select(_ => Disposable.Empty),
                Observable.FromEventPattern<ClosingEventArgs>(
                    x => PhoneApplicationService.Current.Closing += x, x => PhoneApplicationService.Current.Closing -= x)
                    .Select(_ => Disposable.Empty));

            this.ShouldInvalidateState =
                Observable.FromEventPattern<ApplicationUnhandledExceptionEventArgs>(x => app.UnhandledException += x, x => app.UnhandledException -= x)
                    .Select(_ => Unit.Default);
        }

        public IObservable<string> IsLaunchingNew
        {
            get;
            private set;
        }

        public IObservable<string> IsResuming
        {
            get;
            private set;
        }

        public IObservable<string> IsUnpausing
        {
            get;
            private set;
        }

        public IObservable<Unit> ShouldInvalidateState
        {
            get;
            private set;
        }

        public IObservable<IDisposable> ShouldPersistState
        {
            get;
            private set;
        }

        public void SetupDefaultSuspendResume(ISuspensionDriver driver = null)
        {

        }
    }
}
