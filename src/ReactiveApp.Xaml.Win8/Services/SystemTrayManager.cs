using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ReactiveApp.Xaml.Services
{
    public class SystemTrayManager : ISystemTrayManager
    {
        private SystemTray systemTray;

        private static Lazy<SystemTrayManager> instance = new Lazy<SystemTrayManager>(() => new SystemTrayManager());

        public static ISystemTrayManager Instance
        {
            get { return instance.Value; }
        }

        /// <summary>
        /// Initializes the <see cref="OrientationManager"/> class.
        /// </summary>
        private SystemTrayManager()
        {
            this.systemTray = new SystemTray();
            Canvas.SetZIndex(systemTray, 20000);
            this.ProgressIndicator = new ProgressIndicator(systemTray);

            var color = ((SolidColorBrush)Application.Current.Resources["ApplicationForegroundThemeBrush"]).Color;
            this.Foreground = color;
            this.ProgressIndicator.Foreground = color;
            this.Opacity = 1.0;
            this.Background = Colors.Red;
            this.IsVisible = true;
        }

        public Color Foreground
        {
            get
            {
                SolidColorBrush brush = this.systemTray.Foreground as SolidColorBrush;
                if (brush != null)
                {
                    return brush.Color;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            set
            {
                this.systemTray.Foreground = new SolidColorBrush(value);
            }
        }

        public Color Background
        {
            get
            {
                SolidColorBrush brush = this.systemTray.Background as SolidColorBrush;
                if (brush != null)
                {
                    return brush.Color;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
            set
            {
                this.systemTray.Background = new SolidColorBrush(value);
            }
        }

        public double Opacity
        {
            get { return this.systemTray.TrayOpacity; }
            set { this.systemTray.TrayOpacity = value; }
        }

        public bool IsVisible
        {
            get { return this.systemTray.Visibility == Visibility.Visible; }
            set { this.systemTray.Visibility = value ?  Visibility.Visible : Visibility.Collapsed; }
        }

        public IProgressIndicator ProgressIndicator
        {
            get;
            private set;
        }
    }
}
