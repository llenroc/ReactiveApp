using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using ReactiveApp.Services;

namespace ReactiveApp.Xaml.Services
{
    public class OrientationManager : IPhoneOrientationManager
    {
        private PhoneApplicationFrame frame;

        private static DisplayOrientations preferred = DisplayOrientations.Landscape | DisplayOrientations.Portrait;

        private static Lazy<OrientationManager> instance = new Lazy<OrientationManager>(() => new OrientationManager());

        internal static IPhoneOrientationManager InternalInstance
        {
            get { return instance.Value; }
        }

        public static IOrientationManager Instance
        {
            get { return instance.Value; }
        }

        private OrientationManager()
        { 
        }

        void IPhoneOrientationManager.Initialize(PhoneApplicationFrame frame)
        {
            Contract.Requires<ArgumentNullException>(frame != null, "frame");

            this.frame = frame;

            OrientationChanged = Observable.FromEventPattern<OrientationChangedEventArgs>(h => frame.OrientationChanged += h, h => frame.OrientationChanged -= h)
                .Select(_ => _.EventArgs.Orientation.AsDisplayOrientations()).Publish().RefCount();

            Observable.FromEventPattern<NavigatedEventHandler, NavigationEventArgs>(h => frame.Navigated += h, h => frame.Navigated -= h)
                .Where(ep => ep.EventArgs.Content != null).Subscribe(ep =>
                {
                    PhoneApplicationPage page = ep.EventArgs.Content as PhoneApplicationPage;
                    if (page != null)
                    {
                        page.SupportedOrientations = preferred.AsSupportedPageOrientation();
                    }
                });
        }

        public DisplayOrientations Orientation
        {
            get { return frame.Orientation.AsDisplayOrientations(); }
        }

        public DisplayOrientations PreferredOrientation
        {
            get { return preferred; }
            set
            {
                preferred = value;
                PhoneApplicationPage page = frame.Content as PhoneApplicationPage;
                if (page != null)
                {
                    page.SupportedOrientations = preferred.AsSupportedPageOrientation();
                }
            }
        }

        public IObservable<DisplayOrientations> OrientationChanged { get; private set; }
    }

    public static class OrientationExtensions
    {
        public static DisplayOrientations AsDisplayOrientations(this PageOrientation o)
        {
            switch (o)
            {
                case PageOrientation.None:
                    return DisplayOrientations.None;
                case PageOrientation.LandscapeRight:
                    return DisplayOrientations.LandscapeFlipped;
                case PageOrientation.Portrait:
                case PageOrientation.PortraitUp:
                    return DisplayOrientations.Portrait;
                case PageOrientation.PortraitDown:
                    return DisplayOrientations.PortraitFlipped;
                case PageOrientation.Landscape:
                case PageOrientation.LandscapeLeft:
                default:
                    return DisplayOrientations.Landscape;
            }
        }

        public static SupportedPageOrientation AsSupportedPageOrientation(this DisplayOrientations o)
        {
            if ((int)o % 3 == 0)
            {
                return SupportedPageOrientation.PortraitOrLandscape;
            }
            else if ((o & DisplayOrientations.Landscape) == DisplayOrientations.Landscape || (o & DisplayOrientations.LandscapeFlipped) == DisplayOrientations.LandscapeFlipped)
            {
                return SupportedPageOrientation.Landscape;
            }
            else
            {
                return SupportedPageOrientation.Portrait;
            }
        }
    }
}
