
namespace ReactiveApp.Navigation
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
