using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !WINDOWS_PHONE
using Windows.UI;
#else
using System.Windows.Media;
#endif

namespace ReactiveApp.Xaml.Controls
{
    public interface IProgressIndicator
    {  
        /// <summary>
        /// Gets or sets a value that indicates whether the progress indicator on the
        /// system tray on the current view is determinate or indeterminate.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the progress indicator is indeterminate; <c>false</c> if the progress bar
        ///   is determinate.
        /// </value>
        bool IsIndeterminate { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the progress indicator on the system tray
        /// on the current view.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the progress indicator is visible; otherwise, <c>false</c>.
        /// </value>
        bool IsVisible { get; set; }

        /// <summary>
        /// Gets or sets the text of the progress indicator on the system tray on the
        /// current view.
        /// </summary>
        /// <value>
        /// The text of the progress indicator on the system tray.
        /// </value>
        string Text { get; set; }
 
        /// <summary>
        /// Gets or sets the value of the progress indicator on the system tray on the
        /// current view.
        /// </summary>
        /// <value>
        /// The value of the progress indicator on the system tray.
        /// </value>
        double Value { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the progress indicator on the system tray
        /// on the current view. Note: On Windows Phone the foreground cannot be changed.
        /// </summary>
        /// <value>
        /// The value of the foreground color on the system tray.
        /// </value>
        Color Foreground { get; set; }
    }
}
