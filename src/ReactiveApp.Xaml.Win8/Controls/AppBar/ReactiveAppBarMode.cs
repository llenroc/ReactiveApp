using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveApp.Xaml.Controls
{
    /// <summary>
    /// Specifies the different modes of the ReactiveAppBar when it is not opened.
    /// </summary>
    public enum ReactiveAppBarMode
    {
        /// <summary>
        /// No visual indication of the existence of the appbar.
        /// </summary>
        Hidden,

        /// <summary>
        /// Visual indication of the appbar similar to the Minimized mode on Windows Phone.
        /// </summary>
        Minimized,

        /// <summary>
        /// Shows only the appbar buttons without the text associated to the buttons.
        /// </summary>
        Expanded,
    }
}
