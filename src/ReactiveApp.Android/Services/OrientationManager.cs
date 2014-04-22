using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using ReactiveApp.Services;

namespace ReactiveApp.Android.Services
{
    internal class OrientationManager : Java.Lang.Object, IAndroidOrientationManager, Application.IActivityLifecycleCallbacks
    {
        private Application application;
        private Activity currentActivity;

        private readonly ISubject<DisplayOrientations> orientationChanged;
        private OrientationEventListener listener;
        private DisplayOrientations preferred = DisplayOrientations.None;

        private static Lazy<OrientationManager> instance = new Lazy<OrientationManager>(() => new OrientationManager());

        public static IOrientationManager Instance
        {
            get { return instance.Value; }
        }

        private OrientationManager()
        {
            this.orientationChanged = new Subject<DisplayOrientations>();
        }

        public void Initialize(Application application)
        {
            this.application = application;
            this.application.RegisterActivityLifecycleCallbacks(this);
            this.listener = new OrientationEventListenerImpl(this.application.ApplicationContext, this);
        }

        public DisplayOrientations Orientation
        {
            get
            {
                IWindowManager window = this.application.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
                SurfaceOrientation rotation = window.DefaultDisplay.Rotation;
                DisplayMetrics dm = new DisplayMetrics();
                window.DefaultDisplay.GetMetrics(dm);
                int width = dm.WidthPixels;
                int height = dm.HeightPixels;

                DisplayOrientations orientation;
                // if the device's natural orientation is portrait:
                if ((rotation == SurfaceOrientation.Rotation0 || rotation == SurfaceOrientation.Rotation180) && height > width ||
                    (rotation == SurfaceOrientation.Rotation90 || rotation == SurfaceOrientation.Rotation270) && width > height)
                {
                    switch (rotation)
                    {
                        case SurfaceOrientation.Rotation0:
                            orientation = DisplayOrientations.Portrait;
                            break;
                        case SurfaceOrientation.Rotation90:
                            orientation = DisplayOrientations.Landscape;
                            break;
                        case SurfaceOrientation.Rotation180:
                            orientation = DisplayOrientations.PortraitFlipped;
                            break;
                        case SurfaceOrientation.Rotation270:
                            orientation = DisplayOrientations.LandscapeFlipped;
                            break;
                        default:
                            orientation = DisplayOrientations.Portrait;
                            break;
                    }
                }
                // if the device's natural orientation is landscape or if the device
                // is square:
                else
                {
                    switch (rotation)
                    {
                        case SurfaceOrientation.Rotation0:
                            orientation = DisplayOrientations.Landscape;
                            break;
                        case SurfaceOrientation.Rotation90:
                            orientation = DisplayOrientations.Portrait;
                            break;
                        case SurfaceOrientation.Rotation180:
                            orientation = DisplayOrientations.LandscapeFlipped;
                            break;
                        case SurfaceOrientation.Rotation270:
                            orientation = DisplayOrientations.PortraitFlipped;
                            break;
                        default:
                            orientation = DisplayOrientations.Landscape;
                            break;
                    }
                }

                return orientation;
            }
        }

        public DisplayOrientations PreferredOrientation
        {
            get { return preferred; }
            set
            {
                preferred = value;
                this.ApplyRequestedOrientation();
            }
        }

        public IObservable<DisplayOrientations> OrientationChanged
        {
            get { return this.orientationChanged.DistinctUntilChanged(); }
        }

        private void ApplyRequestedOrientation()
        {
            if (this.currentActivity != null)
            {
                this.currentActivity.RequestedOrientation = preferred.AsSupportedScreenOrientation();
            }
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            if (this.currentActivity != activity)
            {
                this.currentActivity = activity;
                this.ApplyRequestedOrientation();
            }
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
        }

        public void OnActivityResumed(Activity activity)
        {
            if (this.currentActivity != activity)
            {
                this.currentActivity = activity;
                this.ApplyRequestedOrientation();
            }
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            if (this.currentActivity != activity)
            {
                this.currentActivity = activity;
                this.ApplyRequestedOrientation();
            }
        }

        public void OnActivityStopped(Activity activity)
        {
        }

        class OrientationEventListenerImpl : OrientationEventListener
        {
            private readonly OrientationManager manager;

            public OrientationEventListenerImpl(Context context, OrientationManager manager)
                : base(context)
            {
                this.manager = manager;
            }

            public override void OnOrientationChanged(int orientation)
            {
                this.manager.orientationChanged.OnNext(manager.Orientation);
            }
        }        
    }

    public static class DisplayOrientationsExtensions
    {
        public static ScreenOrientation AsSupportedScreenOrientation(this DisplayOrientations o)
        {
            switch (o)
            {
                case DisplayOrientations.None:
                    return ScreenOrientation.Unspecified;
                    break;
                case DisplayOrientations.Landscape:
                    return ScreenOrientation.Landscape;
                    break;
                case DisplayOrientations.Portrait:
                    return ScreenOrientation.Portrait;
                    break;
                case DisplayOrientations.LandscapeFlipped:
                    return ScreenOrientation.ReverseLandscape;
                    break;
                case DisplayOrientations.PortraitFlipped:
                    return ScreenOrientation.ReversePortrait;
                    break;
                case DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped:
                    return ScreenOrientation.SensorLandscape;
                    break;
                case DisplayOrientations.Portrait | DisplayOrientations.PortraitFlipped:
                    return ScreenOrientation.SensorPortrait;
                    break;
                default:
                    return ScreenOrientation.Unspecified;
                    break;
            }
        }            
    }
}