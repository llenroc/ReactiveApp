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
        public bool Cancel { get; set; }

        public NavigationMode NavigationMode { get; private set; }

        public IJournalEntry Entry { get; private set; }

        public NavigatingInfo(NavigationMode navigationMode, IJournalEntry entry)
        {
            this.NavigationMode = navigationMode;
            this.Entry = entry;
        }
    }
}
