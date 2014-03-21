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
        public object Content { get; private set; }

        public NavigationMode NavigationMode { get; private set; }
        
        public IJournalEntry Entry { get; private set; }
        
        public NavigatedInfo(object content, NavigationMode navigationMode, IJournalEntry entry)
        {
            this.Content = content;
            this.NavigationMode = navigationMode;
            this.Entry = entry;
        }
    }
}
