using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Util;
using ReactiveApp.Services;

namespace ReactiveApp.Android.Services
{
    public class SuspensionService : Java.Lang.Object, ISuspensionService, IAndroidCurrentActivity, Application.IActivityLifecycleCallbacks
    {
        private Application application;
        private Activity currentActivity;

        private int activeActivities;

        private readonly ISubject<string> isLaunchingNew = new Subject<string>();
        private readonly ISubject<string> isResuming = new Subject<string>();
        private readonly ISubject<IDisposable> shouldPersistState = new Subject<IDisposable>();
        private readonly ISubject<Unit> shouldInvalidateState = new Subject<Unit>();
        
        private readonly ISubject<Activity> currentActivitySubject;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SuspensionService"/> class.
        /// Based on WinRTSuspensionHost in ReactiveUI.Mobile
        /// </summary>
        public SuspensionService(Application application)
        {
            //Contract.Requires<ArgumentNullException>(application != null, "application");

            this.application = application;

            this.isLaunchingNew = new Subject<string>();
            this.isLaunchingNew.Subscribe(_ => Log.Debug("Test", "IsLaunchingNew"));
            this.isResuming = new Subject<string>();
            this.isResuming.Subscribe(_ => Log.Debug("Test", "IsResuming"));
            this.shouldPersistState = new Subject<IDisposable>();
            this.shouldPersistState.Subscribe(_ => Log.Debug("Test", "ShouldPersistState"));
            this.shouldInvalidateState = new Subject<Unit>();
            this.shouldInvalidateState.Subscribe(_ => Log.Debug("Test", "ShouldInvalidateState"));
            
            this.currentActivitySubject = new Subject<Activity>();

            this.application.RegisterActivityLifecycleCallbacks(this);
        }

        public IObservable<string> IsLaunchingNew
        {
            get { return this.isLaunchingNew; }
        }

        public IObservable<string> IsResuming
        {
            get { return this.isResuming; }
        }

        public IObservable<string> IsUnpausing
        {
            get { return this.isResuming; }
        }

        public IObservable<Unit> ShouldInvalidateState
        {
            get { return this.shouldInvalidateState; }
        }

        public IObservable<IDisposable> ShouldPersistState
        {
            get { return this.shouldPersistState; }
        }

        public IObservable<Activity> CurrentActivity
        {
            get { return this.currentActivitySubject.DistinctUntilChanged(); }
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            Log.Debug("Test", "Activity {0} created.", activity.GetHashCode());
            if (this.currentActivity == null)
            {
                this.isLaunchingNew.OnNext(string.Empty);
            }
            this.currentActivity = activity;

            this.currentActivitySubject.OnNext(activity);
        }

        public void OnActivityDestroyed(Activity activity)
        {
            // not guaranteed to be called 
            Log.Debug("Test", "Activity {0} destroyed.", activity.GetHashCode());
        }

        public void OnActivityPaused(Activity activity)
        {
            Log.Debug("Test", "Activity {0} paused.", activity.GetHashCode());
        }

        public void OnActivityResumed(Activity activity)
        {
            Log.Debug("Test", "Activity {0} resumed.", activity.GetHashCode());

            this.currentActivitySubject.OnNext(activity);
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
            Log.Debug("Test", "Activity {0} saveinstancestate.", activity.GetHashCode());
        }

        public void OnActivityStarted(Activity activity)
        {
            Log.Debug("Test", "Activity {0} started.", activity.GetHashCode());
            if (this.activeActivities++ == 0)
            {
                this.isResuming.OnNext(string.Empty);
            }
            this.currentActivity = activity;

            this.currentActivitySubject.OnNext(activity);
        }

        public void OnActivityStopped(Activity activity)
        {
            // not guaranteed to be called pre HoneyComb (API Level 11)
            Log.Debug("Test", "Activity {0} stopped.", activity.GetHashCode());
            if (--this.activeActivities == 0)
            {
                this.shouldPersistState.OnNext(Disposable.Empty);
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.application.UnregisterActivityLifecycleCallbacks(this);
            this.application = null;
        }
    }
}
