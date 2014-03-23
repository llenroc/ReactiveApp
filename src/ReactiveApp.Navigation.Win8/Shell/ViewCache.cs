using System.Collections.Generic;

namespace ReactiveApp.Navigation
{
    internal class ViewCache
    {
        private readonly Dictionary<IJournalEntry, ReactiveView> entryToViewMap;

        #region Constructors

        public ViewCache()
        {
            entryToViewMap = new Dictionary<IJournalEntry, ReactiveView>();
        }

        #endregion

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

        public void Store(IJournalEntry entry, ReactiveView view)
        {
            //switch (view.NavigationCacheMode)
            //{
            //    case NavigationCacheMode.Backward:
            //    case NavigationCacheMode.BackwardAndForward:
            //    case NavigationCacheMode.Forward:
            //        JournalEntry journalEntry = entry as JournalEntry;
            //        if (journalEntry != null)
            //        {
            //            journalEntry.State = view;
            //        }
            //        else
            //        {
            //            throw new InvalidOperationException("Expected JournalEntry");
            //        }
            //        break;
            //    case NavigationCacheMode.Enabled:
            //        entryToViewMap[entry] = view;
            //        break;
            //    case NavigationCacheMode.Inherit:
            //    case NavigationCacheMode.Disabled:
            //    default:
            //        break;
            //}
        }
    }
}
