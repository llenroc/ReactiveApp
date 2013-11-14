using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public class NavigatedInfo
    {
        /// <summary>
        /// Gets the root node of the target page's content.
        /// </summary>
        /// <value>
        /// The root node of the target page's content.
        /// </value>
        public object Content { get; private set; }

        /// <summary>
        /// Gets a value that indicates the direction of page navigation.
        /// </summary>
        /// <value>
        /// A value of the enumeration.
        /// </value>
        public NavigationMode NavigationMode { get; private set; }
        
        /// <summary>
        /// Gets the data type of the target page.
        /// </summary>
        /// <value>
        /// The data type of the target page, represented as namespace.type or simply type.
        /// </value>
        public IJournalEntry Entry { get; private set; }

        /// <summary>
        /// Gets a value indicating whether this navigation is initiated by the app.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is navigation initiator; otherwise, <c>false</c>.
        /// </value>
        public bool IsNavigationInitiator { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AlternativeNavigationEventArgs" /> class.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="navigationMode">The navigation mode.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="sourcePageType">Type of the source page.</param>
        public NavigatedInfo(object content, NavigationMode navigationMode, IJournalEntry entry, bool isNavigationInitiator)
        {
            this.Content = content;
            this.NavigationMode = navigationMode;
            this.Entry = entry;
            this.IsNavigationInitiator = isNavigationInitiator;
        }
    }
}
