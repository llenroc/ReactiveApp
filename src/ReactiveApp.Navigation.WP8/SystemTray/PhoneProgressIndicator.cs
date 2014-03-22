using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Microsoft.Phone.Shell;

namespace ReactiveApp.Navigation
{
    internal class PhoneProgressIndicator : IProgressIndicator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneProgressIndicator"/> class.
        /// </summary>
        public PhoneProgressIndicator()
        {
            this.Indicator = new ProgressIndicator();
        }

        public ProgressIndicator Indicator { get; set; }

        public bool IsIndeterminate
        {
            get { return this.Indicator.IsIndeterminate; }
            set { this.Indicator.IsIndeterminate = value; }
        }

        public bool IsVisible
        {
            get { return this.Indicator.IsVisible; }
            set { this.Indicator.IsVisible = value; }
        }

        public string Text
        {
            get { return this.Indicator.Text; }
            set { this.Indicator.Text = value; }
        }

        public double Value
        {
            get { return this.Indicator.Value; }
            set { this.Indicator.Value = value; }
        }

        public Color Foreground
        {
            get { return (Color)Application.Current.Resources["PhoneAccentColor"]; }
            set { }
        }
    }
}
