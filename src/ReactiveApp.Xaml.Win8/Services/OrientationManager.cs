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
            OrientationChanged = Observable.FromEventPattern<Windows.Graphics.Display.DisplayPropertiesEventHandler, object>(
                h => Windows.Graphics.Display.DisplayProperties.OrientationChanged += h, 
                h => Windows.Graphics.Display.DisplayProperties.OrientationChanged -= h)
                .Select(_ => (DisplayOrientations)Windows.Graphics.Display.DisplayProperties.CurrentOrientation).Publish().RefCount();
        }

        public DisplayOrientations Orientation
        {
            get
            {
                return (DisplayOrientations)Windows.Graphics.Display.DisplayProperties.CurrentOrientation;
            }
        }

        public DisplayOrientations PreferredOrientation
        {
            get { return (DisplayOrientations)Windows.Graphics.Display.DisplayProperties.AutoRotationPreferences; }
            set { Windows.Graphics.Display.DisplayProperties.AutoRotationPreferences = (Windows.Graphics.Display.DisplayOrientations)value; }
        }

        public IObservable<DisplayOrientations> OrientationChanged { get; private set; }
    }
}
