using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;

namespace ReactiveApp.Xaml.Services
{
    public class OrientationManager : IOrientationManager
    {
        private static DisplayInformation di;

        private static Lazy<OrientationManager> instance = new Lazy<OrientationManager>(() => new OrientationManager());

        public static IOrientationManager Instance
        {
            get { return instance.Value; }
        }

        private OrientationManager()
        {
            di = DisplayInformation.GetForCurrentView();
            OrientationChanged = WindowsObservable.FromEventPattern<DisplayInformation, object>(h => di.OrientationChanged += h, h => di.OrientationChanged -= h)
                .Select(_ => di.CurrentOrientation).Publish().RefCount();
        }

        public DisplayOrientations Orientation
        {
            get
            {
                return di.CurrentOrientation;
            }
        }

        public DisplayOrientations PreferredOrientation
        {
            get { return DisplayInformation.AutoRotationPreferences; }
            set { DisplayInformation.AutoRotationPreferences = value; }
        }

        public IObservable<DisplayOrientations> OrientationChanged { get; private set; }
    }
}
