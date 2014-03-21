using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using ReactiveApp.Xaml.Controls;

namespace ReactiveApp.Xaml.Services
{
    public class OrientationManager : IOrientationManager
    {
        private static ReactiveApplication rxApp;
        private static DisplayOrientations preferred = DisplayOrientations.Landscape | DisplayOrientations.Portrait;

        private static Lazy<OrientationManager> instance = new Lazy<OrientationManager>(() => new OrientationManager());

        public static IOrientationManager Instance
        {
            get { return instance.Value; }
        }

        private OrientationManager()
        {
            rxApp = Application.Current as ReactiveApplication;

            if (rxApp == null)
            {
                throw new InvalidOperationException("OrientationManager requires ReactiveApplication.");
            }
                
            OrientationChanged = Observable.FromEventPattern<OrientationChangedEventArgs>(h => rxApp.frameHelper.Frame.OrientationChanged += h, h => rxApp.frameHelper.Frame.OrientationChanged -= h)
                .Select(_ => _.EventArgs.Orientation.AsDisplayOrientations()).Publish().RefCount();

            Observable.FromEventPattern<NavigatedEventHandler, NavigationEventArgs>(h => rxApp.frameHelper.Frame.Navigated += h, h => rxApp.frameHelper.Frame.Navigated -= h)
                .Where(ep => ep.EventArgs.Content != null).Subscribe(ep =>
            {
                PhoneApplicationPage page = ep.EventArgs.Content as PhoneApplicationPage;
                if (page != null && page.GetType() == typeof(BackwardsCompatibilityPage))
                {
                    page.SupportedOrientations = preferred.AsSupportedPageOrientation();
                }
            });
        }

        public static DisplayOrientations Orientation
        {
            get { return rxApp.frameHelper.Frame.Orientation.AsDisplayOrientations(); }
        }

        public static DisplayOrientations PreferredOrientation
        {
            get { return preferred; }
            set
            {
                preferred = value;
                PhoneApplicationPage page = rxApp.frameHelper.Frame.Content as PhoneApplicationPage;
                if (page != null)
                {
                    page.SupportedOrientations = preferred.AsSupportedPageOrientation();
                }
            }
        }

        public static IObservable<DisplayOrientations> OrientationChanged { get; private set; }
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
