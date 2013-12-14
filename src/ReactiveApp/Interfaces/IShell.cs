using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
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
        IObservable<bool> BackViewAsync(IJournalEntry entry);

        IObservable<bool> BackViewAsync<V>(V view, object parameter = null) where V : U;

        IObservable<bool> CanBackView { get; }

        IObservable<bool> ViewAsync(IJournalEntry entry);

        IObservable<bool> ViewAsync<V>(V view, object parameter = null) where V : U;

        IObservable<bool> CanView { get; }

        IObservable<bool> ForwardViewAsync(IJournalEntry entry);

        IObservable<bool> ForwardViewAsync<V>(V view, object parameter = null) where V : U;

        IObservable<bool> CanForwardView { get; }

        IReactiveList<IJournalEntry> BackStack { get; }

        IReactiveList<IJournalEntry> ForwardStack { get; }

        IObservable<NavigatingInfo> Navigating { get; }

        IObservable<NavigatedInfo> Navigated { get; }

        IObservable<IJournalEntry> ViewJournal { get; }

        /// <summary>
        /// Gets the stream of views. Subscribing to this observable should always return a value immediately.
        /// </summary>
        /// <value>
        /// The view.
        /// </value>
        IObservable<U> View { get; }

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
            return This.ViewAsync((IJournalEntry)new JournalEntry(viewType, parameter));
        }

        public static IObservable<bool> GoBackAsync<T, U>(this IShell<T, U> This)
            where T : class, IShell<T, U>
            where U : class, IView<T, U>
        {
            return This.BackViewAsync(This.BackStack.Last());
        }

        public static IObservable<bool> GoForwardAsync<T, U>(this IShell<T, U> This)
            where T : class, IShell<T, U>
            where U : class, IView<T, U>
        {
            return This.ForwardViewAsync(This.ForwardStack.Last());
        }
    }
}
