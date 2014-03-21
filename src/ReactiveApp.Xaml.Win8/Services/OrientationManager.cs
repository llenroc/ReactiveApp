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
        private static Lazy<OrientationManager> instance = new Lazy<OrientationManager>(() => new OrientationManager());

        public static IOrientationManager Instance
        {
            get { return instance.Value; }
        }

        /// <summary>
        /// Initializes the <see cref="OrientationManager"/> class.
        /// </summary>
        private OrientationManager()
        {
            OrientationChanged = Observable.FromEventPattern<DisplayPropertiesEventHandler, object>(h => DisplayProperties.OrientationChanged += h, h => DisplayProperties.OrientationChanged -= h)
                .Select(_ => DisplayProperties.CurrentOrientation).Publish().RefCount();
        }

        public DisplayOrientations Orientation
        {
            get
            {
                return DisplayProperties.CurrentOrientation;
            }
        }

        public DisplayOrientations PreferredOrientation
        {
            get { return DisplayProperties.AutoRotationPreferences; }
            set { DisplayProperties.AutoRotationPreferences = value; }
        }

        public IObservable<DisplayOrientations> OrientationChanged { get; private set; }
    }
}
