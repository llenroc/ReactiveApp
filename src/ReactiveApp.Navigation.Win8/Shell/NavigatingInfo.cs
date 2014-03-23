
namespace ReactiveApp.Navigation
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
