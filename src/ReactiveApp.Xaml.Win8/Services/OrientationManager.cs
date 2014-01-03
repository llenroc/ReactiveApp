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
        /// <summary>
        /// Initializes the <see cref="OrientationManager"/> class.
        /// </summary>
        static OrientationManager()
        {
            OrientationChanged = Observable.FromEventPattern<DisplayPropertiesEventHandler, object>(h => DisplayProperties.OrientationChanged += h, h => DisplayProperties.OrientationChanged -= h)
                .Select(_ => DisplayProperties.CurrentOrientation).Publish().RefCount();
        }

        public static DisplayOrientations Orientation
        {
            get
            {
                return DisplayProperties.CurrentOrientation;
            }
        }

        public static DisplayOrientations PreferredOrientation
        {
            get { return DisplayProperties.AutoRotationPreferences; }
            set { DisplayProperties.AutoRotationPreferences = value; }
        }

        public static IObservable<DisplayOrientations> OrientationChanged { get; private set; }
    }
}
