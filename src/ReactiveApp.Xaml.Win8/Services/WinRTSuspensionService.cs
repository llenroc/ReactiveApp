using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

using ReactiveApp.Interfaces;
using ReactiveApp.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace ReactiveApp.Xaml.Services
{
    public class WinRTSuspensionService : ISuspensionService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WinRTSuspensionService"/> class.
        /// Based on WinRTSuspensionHost in ReactiveUI.Mobile
        /// </summary>
        public WinRTSuspensionService(Application app, ISubject<LaunchActivatedEventArgs> launched)
        {
            var launchNew = new[] { ApplicationExecutionState.ClosedByUser, ApplicationExecutionState.NotRunning, };
            this.IsLaunchingNew = launched
                .Where(x => launchNew.Contains(x.PreviousExecutionState))
                .Select(x => x.Arguments);

            this.IsResuming = launched
                .Where(x => x.PreviousExecutionState == ApplicationExecutionState.Terminated)
                .Select(x => x.Arguments);

            var unpausing = new[] { ApplicationExecutionState.Suspended, ApplicationExecutionState.Running, };
            this.IsUnpausing = launched
                .Where(x => unpausing.Contains(x.PreviousExecutionState))
                .Select(x => x.Arguments);

            var shouldPersistState = new Subject<SuspendingEventArgs>();
            app.Suspending += (o, e) => shouldPersistState.OnNext(e);
            this.ShouldPersistState =
                shouldPersistState.Select(x =>
                {
                    var deferral = x.SuspendingOperation.GetDeferral();
                    return Disposable.Create(deferral.Complete);
                });

            var shouldInvalidateState = new Subject<Unit>();
            app.UnhandledException += (o, e) => shouldInvalidateState.OnNext(Unit.Default);
            this.ShouldInvalidateState = shouldInvalidateState;
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
    }
}
