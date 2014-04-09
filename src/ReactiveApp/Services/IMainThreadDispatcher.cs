using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveApp.Services
{
    public interface IMainThreadDispatcher
    {
        IObservable<Unit> RunOnMainThread(Action action);

        IObservable<T> RunOnMainThread<T>(Func<T> func);

        IObservable<Unit> RunOnMainThread<T>(Func<CancellationToken, Task> task);

        IObservable<T> RunOnMainThread<T>(Func<CancellationToken, Task<T>> task);
    }
}
