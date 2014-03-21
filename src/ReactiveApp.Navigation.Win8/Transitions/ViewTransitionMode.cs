using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Xaml.Controls
{
    /// <summary>
    /// Determines whether page transition animations should occur sequentially or parallel/concurrently on both pages.
    /// </summary>
    public enum ViewTransitionMode
    {
        /// <summary>
        /// Out animation runs first, then in animation runs.
        /// </summary>
        Parallel = 0,
        /// <summary>
        /// Both in and out animations start at the same time.
        /// </summary>
        Sequential = 1,
    }
}
