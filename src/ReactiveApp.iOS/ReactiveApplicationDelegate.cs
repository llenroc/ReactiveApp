using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace ReactiveApp.iOS
{
    public class ReactiveApplicationDelegate : UIApplicationDelegate
    {
        internal readonly ISubject<UIApplication> finishedLaunching = new Subject<UIApplication>();
        internal readonly ISubject<UIApplication> willEnterForeground = new Subject<UIApplication>();
        internal readonly Subject<UIApplication> didEnterbackground = new Subject<UIApplication>();
        internal readonly Subject<UIApplication> willTerminate = new Subject<UIApplication>();

        public override void WillEnterForeground(UIApplication application)
        {
            willEnterForeground.OnNext(application);
        }
        
        public override void DidEnterBackground(UIApplication application)
        {
            didEnterbackground.OnNext(application);
        }
        
        public override void WillTerminate(UIApplication application)
        {
            willTerminate.OnNext(application);
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            finishedLaunching.OnNext(application);

            return base.FinishedLaunching(application, launchOptions);
        }
    }
}