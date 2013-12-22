using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;
using ReactiveUI;

namespace ReactiveApp.Interfaces
{
    public interface IShell<T, U>
        where T : class, IShell<T, U>
        where U : class, IView<T, U>
    {
        IObservable<bool> ViewAsync(IJournalEntry entry, NavigationMode mode);

        IObservable<bool> ViewAsync<V>(V view, NavigationMode mode, object parameter = null) where V : U;

        IObservable<bool> IsNavigationActive { get; }

        IReactiveList<IJournalEntry> BackStack { get; }

        IReactiveList<IJournalEntry> ForwardStack { get; }

        IObservable<NavigatingInfo> Navigating { get; }

        IObservable<NavigatedInfo> Navigated { get; }

        IObservable<IJournalEntry> CurrentJournalEntry { get; }

        /// <summary>
        /// Gets the stream of views. Subscribing to this observable should always return a value immediately.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        IObservable<U> CurrentView { get; }

        /// <summary>
        /// Makes the shell visible on screen.
        /// </summary>
        /// <returns></returns>
        IObservable<Unit> Activate();

        /// <summary>
        /// Observable that indicates whether the Shell is visible on screen.
        /// </summary>
        /// <value>
        /// The activated.
        /// </value>
        IObservable<Unit> Activated { get; }

        /// <summary>
        /// Gets or sets a value indicating whether a view journal on the back- and forwardstack is recorded or not.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the journal is disabled; otherwise, <c>false</c>.
        /// </value>
        bool DisableJournal { get; set; }

        /// <summary>
        /// Adds the overlay to the Shell.
        /// </summary>
        /// <param name="overlay">The overlay.</param>
        /// <returns></returns>
        IDisposable AddOverlay(object overlay);
    }

    public static class IShellExtensions
    {
        public static IObservable<bool> NavigateAsync<T, U>(this IShell<T, U> This, Type viewType, object parameter = null)
            where T : class, IShell<T, U>
            where U : class, IView<T, U>
        {
            return This.ViewAsync((IJournalEntry)new JournalEntry(viewType, parameter), NavigationMode.New);
        }

        public static IObservable<bool> GoBackAsync<T, U>(this IShell<T, U> This)
            where T : class, IShell<T, U>
            where U : class, IView<T, U>
        {
            if (This.BackStack.Count > 0)
            {
                return This.ViewAsync(This.BackStack.Last(), NavigationMode.Back);
            }
            else
            {
                return Observable.Return(false);
            }
        }

        public static IObservable<bool> GoForwardAsync<T, U>(this IShell<T, U> This)
            where T : class, IShell<T, U>
            where U : class, IView<T, U>
        {
            if (This.BackStack.Count > 0)
            {
                return This.ViewAsync(This.ForwardStack.Last(), NavigationMode.Forward);
            }
            else
            {
                return Observable.Return(false);
            }
        }
    }
}
