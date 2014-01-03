using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace ReactiveApp.Xaml.Services
{
    public class OrientationManager
    {
        private static DisplayInformation di;
        static OrientationManager()
        {
            di = DisplayInformation.GetForCurrentView();
            OrientationChanged = WindowsObservable.FromEventPattern<DisplayInformation, object>(h => di.OrientationChanged += h, h => di.OrientationChanged -= h)
                .Select(_ => di.CurrentOrientation).Publish().RefCount();
        }

        public static DisplayOrientations Orientation
        {
            get
            {
                return di.CurrentOrientation;
            }
        }

        public static DisplayOrientations PreferredOrientation
        {
            get { return DisplayInformation.AutoRotationPreferences; }
            set { DisplayInformation.AutoRotationPreferences = value; }
        }

        public static IObservable<DisplayOrientations> OrientationChanged { get; private set; }
    }
}
