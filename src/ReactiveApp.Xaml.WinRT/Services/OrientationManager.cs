using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;

namespace ReactiveApp.Xaml.Services
{
    public class OrientationManager : IOrientationManager
    {
        private static Windows.Graphics.Display.DisplayInformation di;

        private static Lazy<OrientationManager> instance = new Lazy<OrientationManager>(() => new OrientationManager());

        public static IOrientationManager Instance
        {
            get { return instance.Value; }
        }

        private OrientationManager()
        {
            di = Windows.Graphics.Display.DisplayInformation.GetForCurrentView();
            OrientationChanged = WindowsObservable.FromEventPattern<Windows.Graphics.Display.DisplayInformation, object>(h => di.OrientationChanged += h, h => di.OrientationChanged -= h)
                .Select(_ => (DisplayOrientations)di.CurrentOrientation).Publish().RefCount();
        }

        public DisplayOrientations Orientation
        {
            get
            {
                return (DisplayOrientations)di.CurrentOrientation;
            }
        }

        public DisplayOrientations PreferredOrientation
        {
            get { return (DisplayOrientations)Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences; }
            set { Windows.Graphics.Display.DisplayInformation.AutoRotationPreferences = (Windows.Graphics.Display.DisplayOrientations)value; }
        }

        public IObservable<DisplayOrientations> OrientationChanged { get; private set; }
    }
}
