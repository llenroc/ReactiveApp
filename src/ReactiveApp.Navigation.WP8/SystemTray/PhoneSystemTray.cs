using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace ReactiveApp.Navigation
{
    internal class PhoneSystemTray : ISystemTray
    {
        private Color foreground;
        private Color background;
        private double opacity;
        private bool isVisible;
        private PhoneProgressIndicator indicator;

        private PhoneApplicationPage page;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneSystemTray"/> class.
        /// </summary>
        public PhoneSystemTray()
        {
            this.foreground = ((SolidColorBrush)Application.Current.Resources["PhoneForegroundBrush"]).Color;
            this.opacity = 0;
            this.isVisible = true;
            this.indicator = new PhoneProgressIndicator();
        }

        /// <summary>
        /// Initializes the SystemTray for the specified page.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <exception cref="System.ArgumentNullException">page</exception>
        internal void Initialize(PhoneApplicationPage page)
        {
            if (page == null)
            {
                throw new ArgumentNullException("page");
            }

            this.page = page;
            SystemTray.SetBackgroundColor(page, background);
            SystemTray.SetForegroundColor(page, foreground);
            SystemTray.SetIsVisible(page, isVisible);
            SystemTray.SetOpacity(page, opacity);
            SystemTray.SetProgressIndicator(page, indicator.Indicator);
        }

        internal void Uninitialize()
        {
            this.page = null;
        }

        /// <summary>
        /// Gets or sets the foreground.
        /// </summary>
        /// <value>
        /// The foreground.
        /// </value>
        public Color Foreground
        {
            get
            {
                if (this.page != null)
                {
                    return SystemTray.GetForegroundColor(page);
                }
                else
                {
                    return this.foreground;
                }
            }
            set
            {
                if (this.page != null)
                {
                    SystemTray.SetForegroundColor(page, foreground);
                }
                else
                {
                    this.foreground = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the background.
        /// </summary>
        /// <value>
        /// The background.
        /// </value>
        public Color Background
        {
            get
            {
                if (this.page != null)
                {
                    return SystemTray.GetBackgroundColor(page);
                }
                else
                {
                    return this.background;
                }
            }
            set
            {
                if (this.page != null)
                {
                    SystemTray.SetBackgroundColor(page, background);
                }
                else
                {
                    this.background = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the opacity.
        /// </summary>
        /// <value>
        /// The opacity.
        /// </value>
        public double Opacity
        {
            get
            {
                if (this.page != null)
                {
                    return SystemTray.GetOpacity(page);
                }
                else
                {
                    return this.opacity;
                }
            }
            set
            {
                if (this.page != null)
                {
                    SystemTray.SetOpacity(page, opacity);
                }
                else
                {
                    this.opacity = value;
                }
            }
        }

        public bool IsVisible
        {
            get
            {
                if (this.page != null)
                {
                    return SystemTray.GetIsVisible(page);
                }
                else
                {
                    return this.isVisible;
                }
            }
            set
            {
                if (this.page != null)
                {
                    SystemTray.SetIsVisible(page, value);
                }
                else
                {
                    this.isVisible = value;
                }
            }
        }

        public IProgressIndicator ProgressIndicator
        {
            get
            {
                return this.indicator;
            }
        }
    }
}
