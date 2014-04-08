using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.Services;

namespace ReactiveApp.iOS.Services
{
    public class SuspensionService : ISuspensionService
    {
        private readonly ReactiveApplicationDelegate application;
        /// <summary>
        /// Initializes a new instance of the <see cref="SuspensionService"/> class.
        /// Based on WinRTSuspensionHost in ReactiveUI.Mobile
        /// </summary>
        public SuspensionService(ReactiveApplicationDelegate application)
        {
            //Contract.Requires<ArgumentNullException>(application != null, "application");

            this.application = application;

            this.IsLaunchingNew = this.application.finishedLaunching.Select(_ => string.Empty);
            this.IsResuming = Observable.Never<string>();
            this.IsUnpausing = this.application.willEnterForeground.Select(_ => string.Empty);
            var untimelyDeath = new Subject<Unit>();
            AppDomain.CurrentDomain.UnhandledException += (o, e) => untimelyDeath.OnNext(Unit.Default);
            this.ShouldInvalidateState = untimelyDeath;
            this.ShouldPersistState = this.application.willTerminate.Merge(this.application.didEnterbackground).SelectMany(app =>
            {
                var taskId = app.BeginBackgroundTask(new NSAction(() => untimelyDeath.OnNext(Unit.Default)));

                // NB: We're being force-killed, signal invalidate instead
                if (taskId == UIApplication.BackgroundTaskInvalid)
                {
                    untimelyDeath.OnNext(Unit.Default);
                    return Observable.Empty<IDisposable>();
                }

                return Observable.Return(Disposable.Create(() => app.EndBackgroundTask(taskId)));
            });
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