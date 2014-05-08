using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;

namespace ReactiveApp.Xaml.Adapters
{
    public class PhoneApplicationFrameArgumentsProvider : IArgumentsProvider
    {
        private readonly PhoneApplicationFrame frame;

        /// <summary>
        /// 
        /// The following navigation events can occur:
        /// SPECIAL
        ///
        /// Startup navigation                                           (cancel):
        /// IsCancelable:                true
        /// IsNavigationInitiator:       false
        /// NavigationMode:              New
        /// Uri:                         <page uri defined in WMAppManifest>
        ///
        /// Fast app resume, only on WP8                                 (ignore first, cancel second):
        /// 1:
        /// IsCancelable:                false
        /// IsNavigationInitiator:       false
        /// NavigationMode:              Reset
        /// Uri:                         <current visible PhoneApplicationPage>
        /// 2:
        /// IsCancelable:                true
        /// IsNavigationInitiator:       true
        /// NavigationMode:              New
        /// Uri:                         <new PhoneApplicationPage>
        ///
        /// Navigation to a launcher or chooser outside of the app       (ignore):
        /// IsCancelable:                false
        /// IsNavigationInitiator:       false
        /// NavigationMode:              New
        /// Uri:                         app://external/
        /// 
        /// Navigation back into the app from a launcher or chooser      (ignore):
        /// IsCancelable:                false
        /// IsNavigationInitiator:       false
        /// NavigationMode:              Back
        /// Uri:                         <current visible PhoneApplicationPage>
        ///
        /// Close the app by back navigation                             (ignore):
        /// IsCancelable:                false
        /// IsNavigationInitiator:       false
        /// NavigationMode:              Back
        /// Uri:                         app://external/
        ///
        /// NORMAL
        ///
        /// Navigation to new page                                       (ignore):
        /// IsCancelable:                true
        /// IsNavigationInitiator:       true
        /// NavigationMode:              New
        /// Uri:                         <new PhoneApplicationPage>  
        ///
        /// Navigation back to previous page                             (ignore):
        /// IsCancelable:                true
        /// IsNavigationInitiator:       true
        /// NavigationMode:              Back
        /// Uri:                         <previous PhoneApplicationPage> 
        /// 
        /// When launching with arguments or fast app resume, 
        /// we cancel the navigation and publish the arguments for use by startup logic.
        /// </summary>
        public PhoneApplicationFrameArgumentsProvider(PhoneApplicationFrame frame)
        {
            this.frame = frame;

            this.Arguments = Observable.FromEventPattern<NavigatingCancelEventHandler, NavigatingCancelEventArgs>(h => this.frame.Navigating += h, h => this.frame.Navigating -= h)
                .Select(ep => ep.EventArgs)
                .Scan<NavigatingCancelEventArgs, NavigatingCancelEventArgs>(null, (prevNav, curNav) =>
                {
                    //cancel if this is the initial navigation or if the previous navigation was reset
                    if ((prevNav != null && prevNav.NavigationMode == NavigationMode.Reset) || (!curNav.IsNavigationInitiator && curNav.IsCancelable))
                    {
                        curNav.Cancel = true;
                    }
                    return curNav;
                }).Select(e => e.Uri.OriginalString).ObserveOnDispatcher().Publish().RefCount();
        }

        public IObservable<string> Arguments { get; private set; }
    }
}
