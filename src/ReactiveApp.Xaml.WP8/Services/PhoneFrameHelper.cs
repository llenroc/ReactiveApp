using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using ReactiveApp.Xaml.Controls;

namespace ReactiveApp.Xaml.Services
{
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
    /// </summary>
    internal class PhoneFrameHelper : IPhoneFrameHelper
    {
        private PhoneApplicationFrame backwardsCompatibilityFrame;
        private PhoneApplicationPage backwardsCompatibilityPage;
        private UIElement content;

        /// <summary>
        /// Used in Fast app resume scenario's where we need to cancel the native navigation after the reset.
        /// </summary>
        private bool cancelNextNativeNavigation = false;

        private readonly ISubject<string> arguments;
        private readonly AsyncSubject<Unit> activated;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneFrameHelper"/> class.
        /// </summary>
        public PhoneFrameHelper(UIElement content)
        {
            this.content = content;

            this.BackwardsCompatibilityFrame = new PhoneApplicationFrame();
            this.BackwardsCompatibilityFrame.UriMapper = new ReactiveAppUriMapper();

            this.arguments = new Subject<string>();
            this.activated = new AsyncSubject<Unit>();
        }
        
        public PhoneApplicationFrame BackwardsCompatibilityFrame
        {
            get { return this.backwardsCompatibilityFrame; }
            set
            {
                //navigation
                this.backwardsCompatibilityFrame = value;
                this.backwardsCompatibilityFrame.Navigating += (o, e) =>
                {
                    if(e.NavigationMode != System.Windows.Navigation.NavigationMode.Reset)
                    {
                        arguments.OnNext(e.Uri.ToString());
                    }
                };
                //hook up code to a PhoneApplicationPage here to access native WP features
                this.backwardsCompatibilityFrame.Navigated += (o, e) =>
                {
                    var newPage = e.Content as PhoneApplicationPage;
                    if (newPage == null)
                    {
                        return;
                    }

                    if (newPage.GetType() == typeof(BackwardsCompatibilityPage))
                    {
                        this.backwardsCompatibilityPage = newPage;
                        //set the legacy ApplicationBar
                        //this.backwardsCompatibilityPage.ApplicationBar = this.CurrentPage.ApplicationBar;
                    }

                    if (this.backwardsCompatibilityPage != null)
                    {
                        if (this.backwardsCompatibilityPage.Content == null)
                        {
                            this.backwardsCompatibilityPage.Content = this.content;
                            if (Application.Current.RootVisual != this.BackwardsCompatibilityFrame)
                            {
                                Application.Current.RootVisual = this.BackwardsCompatibilityFrame;
                            }
                            if (!this.activated.IsCompleted)
                            {
                                this.activated.OnNext(Unit.Default);
                                this.activated.OnCompleted();
                            }
                        }
                    }
                };
            }
        }

        public IObservable<string> Arguments
        {
            get { return this.arguments; }
        }

        class ReactiveAppUriMapper : UriMapperBase
        {
            private readonly Uri backCompatUri;

            /// <summary>
            /// Initializes a new instance of the <see cref="ReactiveAppUriMapper"/> class.
            /// </summary>
            public ReactiveAppUriMapper()
            {
                this.backCompatUri = new Uri("/ReactiveApp.Xaml;component/BackwardsCompatibilityPage.xaml", UriKind.Relative);
            }

            /// <summary>
            /// When overridden in a derived class, converts a requested uniform resource identifier (URI) to a new URI.
            /// </summary>
            /// <param name="uri">The original URI value to be mapped to a new URI.</param>
            /// <returns>
            /// A URI to use for the request instead of the value in the <paramref name="uri" /> parameter.
            /// </returns>
            /// <exception cref="System.NotImplementedException"></exception>
            public override Uri MapUri(Uri uri)
            {
                if (uri.OriginalString.Equals("/ReactiveApp"))
                {
                    return this.backCompatUri;
                }
                else
                {
                    return uri;
                }
            }
        }

        public IObservable<Unit> Activate()
        {
            return this.activated;
        }
    }
}
