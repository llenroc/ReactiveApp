using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;

namespace ReactiveApp.Interfaces
{
    public interface IShell<T, U>
        where T : class, IShell<T, U>
        where U : class, IView<T, U>
    {
        Task BackAsync();

        IObservable<bool> CanBack { get; }

        Task ViewAsync<V>(V view = default(V)) where V : U;

        IObservable<bool> CanView { get; }

        Task ForwardAsync();

        IObservable<bool> CanForward { get; }

        Stack<U> BackStack { get; }

        Stack<U> ForwardStack { get; }

        IObservable<NavigatingInfo> Navigating { get; }

        IObservable<NavigatedInfo> Navigated { get; }

        IObservable<IJournalEntry> ViewJournal { get; }

        Task Activate();
    }
}
