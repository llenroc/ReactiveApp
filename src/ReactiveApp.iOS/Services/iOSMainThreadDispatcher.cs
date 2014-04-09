using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveUI;

namespace ReactiveApp.iOS.Services
{
    public class iOSMainThreadDispatcher : IMainThreadDispatcher
    {
        private IScheduler scheduler;

        public iOSMainThreadDispatcher()
        {
            scheduler = RxApp.MainThreadScheduler;
        }

        public IObservable<Unit> RunOnMainThread(Action action)
        {
            return Observable.Start(action, this.scheduler);
        }

        public IObservable<T> RunOnMainThread<T>(Func<T> func)
        {
            return Observable.Start(func, this.scheduler);
        }

        public IObservable<Unit> RunOnMainThread<T>(Func<CancellationToken, Task> actionAsync)
        {
            return ObservableEx.StartAsync(actionAsync, this.scheduler);
        }

        public IObservable<T> RunOnMainThread<T>(Func<CancellationToken, Task<T>> funcAsync)
        {
            return ObservableEx.StartAsync(funcAsync, this.scheduler);
        }
    }
}
