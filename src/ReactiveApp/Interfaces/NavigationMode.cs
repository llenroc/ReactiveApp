using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Interfaces
{
    /// <summary>
    /// Specifies the type of navigation that is occurring.
    /// </summary>
    public enum NavigationMode
    { 
        /// <summary>
        /// Navigating to new content. This value is used when the <see cref="Methylium.Xaml.Controls.ReactiveFrame.Navigate(System.Type)"/>
        /// method is called. 
        /// </summary>
        New = 0,
        
        /// <summary>
        /// Navigating to the most recent content in the back navigation history. This
        /// value is used when the <see cref="Methylium.Xaml.Controls.ReactiveFrame.GoBack()"/>
        /// method is called.
        /// </summary>
        Back = 1,
 
        /// <summary>
        /// Navigating to the most recent content in the forward navigation history.
        /// This value is used when the <see cref="Methylium.Xaml.Controls.ReactiveFrame.GoForward()"/>
        /// method is called.
        /// </summary>
        Forward = 2,

        /// <summary>
        /// Navigating to the current page after the app has resumed.
        /// This value is used when the app is resumed from a previously paused/tombstoned state.
        /// </summary>
        Refresh = 3,
    }
}
