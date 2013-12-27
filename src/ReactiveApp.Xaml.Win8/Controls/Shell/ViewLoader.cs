using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;

namespace ReactiveApp.Xaml.Controls
{
    internal class ViewLoader
    {
        private readonly ViewCache cache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLoader"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        public ViewLoader(ViewCache cache)
        {
            this.cache = cache;
        }

        /// <summary>
        /// Gets the view.
        /// </summary>
        /// <param name="journalEntry">The journal entry.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public ReactiveView GetView(IJournalEntry journalEntry)
        {
            ReactiveView view;
            if (!cache.TryGet(journalEntry, out view))
            {
                view = Activator.CreateInstance(journalEntry.ViewType) as ReactiveView;
                //set cacheMode.
                //if (view.NavigationCacheMode == NavigationCacheMode.Inherit)
                //{
                //    view.NavigationCacheMode = this.DefaultCacheMode;
                //}
                //store new page
                cache.Store(journalEntry, view);
            }
            if (view == null)
            {
                throw new InvalidOperationException(string.Format("{0} should be of type ReactiveView", journalEntry.ViewType));
            }
            return view;
        }

        public NavigationCacheMode DefaultCacheMode
        {
            get;
            set;
        }
    }
}
