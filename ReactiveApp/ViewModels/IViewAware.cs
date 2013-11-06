using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveApp.ViewModels
{
    /// <summary>
    /// Denotes a class which is aware of its view(s).
    /// </summary>
    public interface IViewAware
    {
        /// <summary>
        /// Attaches a view to this instance.
        /// </summary>
        /// <param name="view">The view.</param>
        void AttachView(object view);

        /// <summary>
        /// Gets a view previously attached to this instance.
        /// </summary>
        /// <returns>The view.</returns>
        object GetView();

        /// <summary>
        /// Raised when a view is attached.
        /// </summary>
        IObservable<ViewAttachedInfo> ViewAttached { get; }
    }
}
