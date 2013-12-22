using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;

namespace ReactiveApp.Xaml.Controls
{
    /// <summary>
    /// Manages retrieval and caching of ReactiveView instances used in ReactiveShell.
    /// ReactiveShell should always get page instances by calling FrameCache.TryGet()
    /// when navigating to a page or preloading one
    /// and store a page by calling FrameCache.Store() when navigating away from it.
    /// FrameCache decides whether a page should actually be cached when calling Store() or whether to ignore the call.
    /// </summary>
    internal class ViewCache
    {
        private readonly Dictionary<IJournalEntry, ReactiveView> entryToViewMap;

        #region Constructors

        public ViewCache()
        {
            entryToViewMap = new Dictionary<IJournalEntry, ReactiveView>();
        }

        #endregion

        /// <summary>
        /// Tries to retrieve the page for the specified JournalEntry.
        /// If one is already cached - it gets it from the cache and returns true.
        /// If one isn't cached - it returns false.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>A ReactivePage.</returns>
        internal bool TryGet(IJournalEntry entry, out ReactiveView cachedView)
        {
            JournalEntry journalEntry = entry as JournalEntry;
            if (journalEntry != null && journalEntry.State != null)
            {
                cachedView = journalEntry.State as ReactiveView;
                return true;
            }

            cachedView = null;
            return entryToViewMap.TryGetValue(entry, out cachedView);
        }

        /// <summary>
        /// Stores the specified page in cache.
        /// </summary>
        /// <param name="view">The page.</param>
        public void Store(IJournalEntry entry, ReactiveView view)
        {
            switch (view.NavigationCacheMode)
            {
                case NavigationCacheMode.Backward:
                case NavigationCacheMode.BackwardAndForward:
                case NavigationCacheMode.Forward:
                    JournalEntry journalEntry = entry as JournalEntry;
                    if (journalEntry != null)
                    {
                        journalEntry.State = view;
                    }
                    else
                    {
                        throw new InvalidOperationException("Expected JournalEntry");
                    }
                    break;
                case NavigationCacheMode.Enabled:
                    entryToViewMap[entry] = view;
                    break;
                case NavigationCacheMode.Inherit:
                case NavigationCacheMode.Disabled:
                default:
                    break;
            }
        }
    }
}
