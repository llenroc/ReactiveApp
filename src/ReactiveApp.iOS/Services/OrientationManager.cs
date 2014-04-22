using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.Services;

namespace ReactiveApp.iOS.Services
{
    public class OrientationManager : IOrientationManager
    {
        private DisplayOrientations preferred = DisplayOrientations.None;

        private static Lazy<OrientationManager> instance = new Lazy<OrientationManager>(() => new OrientationManager());

        public static IOrientationManager Instance
        {
            get { return instance.Value; }
        }

        private OrientationManager()
        {
            this.OrientationChanged = Observable.Never<DisplayOrientations>();
        }

        public DisplayOrientations Orientation
        {
            get { return UIApplication.SharedApplication.StatusBarOrientation.AsDisplayOrientations(); }
        }

        public DisplayOrientations PreferredOrientation
        {
            get;
            set;
        }

        public IObservable<DisplayOrientations> OrientationChanged
        {
            get;
            private set;
        }
    }
        
    public static class UIInterfaceOrientationExtensions
    {
        public static DisplayOrientations AsDisplayOrientations(this UIInterfaceOrientation o)
        {
            switch (o)
            {
                case UIInterfaceOrientation.LandscapeLeft:
                    return DisplayOrientations.Landscape;
                    break;
                case UIInterfaceOrientation.LandscapeRight:
                    return DisplayOrientations.LandscapeFlipped;
                    break;
                case UIInterfaceOrientation.Portrait:
                    return DisplayOrientations.Portrait;
                    break;
                case UIInterfaceOrientation.PortraitUpsideDown:
                    return DisplayOrientations.PortraitFlipped;
                    break;
                default:
                    return DisplayOrientations.Portrait;
                    break;
            }
        }
    }

}