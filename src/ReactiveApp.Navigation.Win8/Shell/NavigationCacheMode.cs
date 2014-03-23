
namespace ReactiveApp.Navigation
{
    /// <summary>
    /// Specifies how the view is cached when used within a shell.
    /// </summary>
    /// <remarks>
    /// You use the NavigationCacheMode enumeration when setting the NavigationCacheMode property
    /// of the  class.
    /// You specify whether a new instance of the page is created for each visit to the page or whether
    /// a previously constructed instance of the page that has been saved in the cache is used for each visit.
    /// <p/>
    /// The default value for the NavigationCacheMode property is Disabled.
    /// Set the NavigationCacheMode property to Enabled or Required when a new instance of the page is not essential for each visit.
    /// By using a cached instance of the page, you can improve the performance of your application and
    /// reduce the load on your server. Set the NavigationCacheMode property to Disabled
    /// if a new instance must be created for each visit. For example, you should not cache a view that
    /// displays information that is unique to each customer.
    /// <p/>
    /// The OnNavigatedToAsync method is called for each request, even when the view is retrieved from the cache.
    /// You should include in this method code that must be executed for each request rather than placing
    /// that code in the View constructor.
    /// </remarks>
    public enum NavigationCacheMode
    {
        /// <summary>
        /// No cachemode is specified. Use the default cache mode defined on the shell.
        /// This mode is interpreted as Disabled.
        /// </summary>
        Inherit,

        /// <summary>
        /// The page is never cached and a new instance of the page is created on each visit.
        /// </summary>
        Disabled,

        /// <summary>
        /// The page is cached and the cached instance is reused for every visit.
        /// </summary>
        Enabled,

        /// <summary>
        /// The page is cached and reused for Back navigations.
        /// </summary>
        Backward,

        /// <summary>
        /// The page is cached  and reused for Forward navigations.
        /// </summary>
        Forward,

        /// <summary>
        /// The page is cached  and reused for Backward and Forward navigations.
        /// </summary>
        BackwardAndForward,
    }
}
