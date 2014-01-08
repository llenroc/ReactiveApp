using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

#if !WINDOWS_PHONE
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.Foundation;
#else
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Diagnostics;
using Splat;
#endif

namespace ReactiveApp.Xaml.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ReactiveAppBarManager : IReactiveAppBarManager, IEnableLogger
    {
        private Popup popup;
        private Panel grid;
        private Canvas dismissLayer;

        private static Lazy<ReactiveAppBarManager> instance = new Lazy<ReactiveAppBarManager>(() => new ReactiveAppBarManager());

        public static IReactiveAppBarManager Instance
        {
            get { return instance.Value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveAppBarManager"/> class.
        /// </summary>
        /// <param name="shell">The shell.</param>
        private ReactiveAppBarManager()
        {
            popup = new Popup();
            grid = new Grid();
            dismissLayer = new Canvas();

#if !WINDOWS_PHONE
            var size = Observable.FromEventPattern<WindowSizeChangedEventHandler, WindowSizeChangedEventArgs>(h => Window.Current.SizeChanged += h, h => Window.Current.SizeChanged -= h)
                .Select(ep => ep.EventArgs.Size)
                .StartWith(new Size(Window.Current.Bounds.Width, Window.Current.Bounds.Height));
#else
            var size = Observable.FromEventPattern(h => Application.Current.Host.Content.Resized += h, h => Application.Current.Host.Content.Resized -= h)
                .Select(ep => new Size(Application.Current.Host.Content.ActualWidth, Application.Current.Host.Content.ActualHeight))
                .StartWith(new Size(Application.Current.Host.Content.ActualWidth, Application.Current.Host.Content.ActualHeight));
#endif
            size.Subscribe(s =>
            {
                grid.Width = dismissLayer.Width = s.Width;
                grid.Height = dismissLayer.Height = s.Height;
            });

            grid.Children.Add(dismissLayer);
            popup.Child = grid;
            popup.IsOpen = true;
        }

        public IDisposable RegisterAppBar(ReactiveAppBar appbar)
        {
            throw new NotImplementedException();
        }

#if DEBUG
        ~ReactiveAppBarManager()
        {
            string debug = string.Format("ReactiveAppBarManager {0} finalised.", this.GetHashCode());
            Debug.WriteLine(debug);
            this.Log().Debug(debug);
        }
#endif
    }
}
