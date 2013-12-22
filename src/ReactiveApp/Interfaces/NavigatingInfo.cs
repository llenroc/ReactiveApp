using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Interfaces
{
    /// <summary>
    /// Provides event data for the OnNavigatingFrom callback that can be used
    /// to cancel a navigation request from origination.
    /// </summary>
    public class NavigatingInfo
    {
        /// <summary>
        /// Specifies whether a pending navigation should be canceled.
        /// </summary>
        /// <value>
        ///   <c>true</c> to cancel the pending cancelable navigation; <c>false</c> to continue with navigation.
        /// </value>
        public bool Cancel { get; set; }

        /// <summary>
        /// Gets the value of the mode parameter from the originating Navigate call.
        /// </summary>
        /// <value>
        /// The value of the mode parameter from the originating Navigate call.
        /// </value>
        public NavigationMode NavigationMode { get; private set; }

        /// <summary>
        /// Gets the value of the SourcePageType parameter from the originating Navigate call.
        /// </summary>
        /// <value>
        /// The value of the SourcePageType parameter from the originating Navigate call.
        /// </value>
        public IJournalEntry Entry { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternativeNavigatingCancelEventArgs" /> class.
        /// </summary>
        /// <param name="navigationMode">The navigation mode.</param>
        /// <param name="sourcePageType">Type of the source page.</param>
        public NavigatingInfo(NavigationMode navigationMode, IJournalEntry entry)
        {
            this.NavigationMode = navigationMode;
            this.Entry = entry;
        }
    }
}
