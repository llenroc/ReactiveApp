using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using Splat;

#if !WINDOWS_PHONE
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;
using Windows.UI.Core;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Input;
#else
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Shell;
using System.Windows.Input;
#endif

namespace ReactiveApp.Navigation
{
    /// <summary>
    /// 
    /// </summary>
    public class ReactiveAppBarManager : IReactiveAppBarManager, IEnableLogger
    {
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
            grid = new Grid();
            dismissLayer = new Canvas();

#if !WINDOWS_PHONE
            var size = Observable.FromEventPattern<WindowSizeChangedEventHandler, WindowSizeChangedEventArgs>(h => Window.Current.SizeChanged += h, h => Window.Current.SizeChanged -= h)
                .Select(ep => ep.EventArgs.Size)
                .StartWith(new Size(Window.Current.Bounds.Width, Window.Current.Bounds.Height));
            var activated = Observable.FromEventPattern<object>(h => Application.Current.Resuming += h, h => Application.Current.Resuming -= h)
                .Select(ep => Unit.Default);
            var dismiss = Observable.FromEventPattern<TappedEventHandler, TappedRoutedEventArgs>(h => dismissLayer.Tapped += h, h => dismissLayer.Tapped -= h)
                .Select(ep => { ep.EventArgs.Handled = true; return Unit.Default; });
            var toggle = Observable.FromEventPattern<WindowActivatedEventHandler, WindowActivatedEventArgs>(h => Window.Current.Activated += h, h => Window.Current.Activated -= h)
                .Where(ep => Window.Current.Content != null)
                .Select(ep => Observable.FromEventPattern<RightTappedEventHandler, RightTappedRoutedEventArgs>(h => Window.Current.Content.RightTapped += h, h => Window.Current.Content.RightTapped -= h))
                .Switch();
#else
            var size = Observable.FromEventPattern(h => Application.Current.Host.Content.Resized += h, h => Application.Current.Host.Content.Resized -= h)
                .Select(ep => new Size(Application.Current.Host.Content.ActualWidth, Application.Current.Host.Content.ActualHeight))
                .StartWith(new Size(Application.Current.Host.Content.ActualWidth, Application.Current.Host.Content.ActualHeight));
            var activated = Observable.FromEventPattern<ActivatedEventArgs>(h => PhoneApplicationService.Current.Activated += h, h => PhoneApplicationService.Current.Activated -= h)
                .Select(ep => Unit.Default);
            var dismiss = Observable.FromEventPattern<GestureEventArgs>(h => dismissLayer.Tap += h, h => dismissLayer.Tap -= h)
                .Select(ep => { ep.EventArgs.Handled = true; return Unit.Default; });
            var toggle = Observable.Empty<Unit>();
#endif
            size.Subscribe(s =>
            {
                grid.Width = dismissLayer.Width = s.Width;
                grid.Height = dismissLayer.Height = s.Height;
            });
            dismiss.Concat(activated).Subscribe(d =>
            {
                this.Log().Info("Appbars closing because of dismiss or deactivation.");
                this.CloseAllLightDismissableAppBars();
                this.Log().Info("Appbars closed because of dismiss or deactivation.");
            });
            toggle.Subscribe(t =>
            {
                this.Log().Info("Appbars state switching.");
                this.SwitchOpenedAndClosedAppBars();
                this.Log().Info("Appbars state switched.");
            });

            grid.Children.Add(dismissLayer);

            //ReactiveApplication app = Application.Current as ReactiveApplication;
            //if(app != null)
            //{
            //    app.Shell.AddOverlay(grid);
            //}
        }

        public IDisposable AddAppBar(ReactiveAppBar appbar)
        {
            if (appbar == null)
            {
                throw new ArgumentNullException("appbar");
            }

            grid.Children.Add(appbar);
            this.Log().Info("Appbar {0} added.", appbar.GetHashCode());

            IDisposable dismiss = appbar.Opened.Merge(appbar.Closed).Subscribe(x => UpdateDismissLayerInteraction());
            return Disposable.Create(() =>
            {
                dismiss.Dispose();
                grid.Children.Remove(appbar);
                this.Log().Info("Appbar {0} removed.", appbar.GetHashCode());
                UpdateDismissLayerInteraction();
            });
        }

        public void SwitchOpenedAndClosedAppBars()
        {
            this.Log().Info("SwitchOpenedAndClosedAppBars");
            foreach (ReactiveAppBar appbar in grid.Children.OfType<ReactiveAppBar>())
            {
                if ((appbar.CanDismiss && appbar.IsOpen) || (appbar.CanOpen && !appbar.IsOpen))
                {
                    appbar.IsOpen = !appbar.IsOpen;
                }
            }
        }

        public void CloseAllLightDismissableAppBars()
        {
            this.Log().Info("CloseAllLightDismissableAppBars");
            foreach (ReactiveAppBar appbar in grid.Children.OfType<ReactiveAppBar>())
            {
                if (appbar.IsLightDismissEnabled || appbar.CanDismiss)
                {
                    appbar.IsOpen = false;
                }
            }
        }

        private void UpdateDismissLayerInteraction()
        {
            this.Log().Info("UpdateDismissLayerInteraction");
            foreach (ReactiveAppBar appbar in grid.Children.OfType<ReactiveAppBar>())
            {
                if (appbar.IsLightDismissEnabled && appbar.IsOpen)
                {
                    dismissLayer.Background = new SolidColorBrush(Colors.Transparent);
                    return;
                }
            }
            dismissLayer.Background = null;
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
