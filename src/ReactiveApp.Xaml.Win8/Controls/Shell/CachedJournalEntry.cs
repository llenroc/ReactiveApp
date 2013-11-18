using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;

namespace ReactiveApp.Xaml.Controls.Shell
{
    internal class CachedJournalEntry : JournalEntry
    {
        /// <summary>
        /// Gets or sets the cached page.
        /// </summary>
        /// <value>
        /// The cached page.
        /// </value>
        public ReactiveView CachedView { get; set; }
    }
}
