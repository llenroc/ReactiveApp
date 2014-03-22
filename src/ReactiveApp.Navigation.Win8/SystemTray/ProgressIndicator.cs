using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace ReactiveApp.Navigation
{
    internal class ProgressIndicator : IProgressIndicator
    {
        private SystemTray tray;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneProgressIndicator"/> class.
        /// </summary>
        public ProgressIndicator(SystemTray tray)
        {
            this.tray = tray;
        }

        public bool IsIndeterminate
        {
            get { return this.tray.ProgressIndicatorIsIndeterminate; }
            set { this.tray.ProgressIndicatorIsIndeterminate = value; }
        }

        public bool IsVisible
        {
            get { return this.tray.ProgressIndicatorVisibility == Visibility.Visible; }
            set { this.tray.ProgressIndicatorVisibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public string Text
        {
            get { return this.tray.Text; }
            set { this.tray.Text = value; }
        }

        public double Value
        {
            get { return this.tray.ProgressIndicatorValue; }
            set { this.tray.ProgressIndicatorValue = value; }
        }

        public Color Foreground
        {
            get
            {
                SolidColorBrush brush = this.tray.ProgressIndicatorForeground as SolidColorBrush;
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
                this.tray.ProgressIndicatorForeground = new SolidColorBrush(value);
            }
        }
    }
}
