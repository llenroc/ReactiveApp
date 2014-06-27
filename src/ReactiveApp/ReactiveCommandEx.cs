using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveApp
{
    public static class ReactiveCommandEx
    {
        public static ReactiveCommand<T> CreateAsyncObservable<T>(IObservable<bool> canExecute, Func<object, IObservable<T>> executeAsync, IScheduler executeScheduler = null, IScheduler scheduler = null)
        {
            return new ReactiveCommand<T>(canExecute, o => Observable.Start(() => executeAsync(o), executeScheduler ?? RxApp.TaskpoolScheduler).Switch(), scheduler);
        }

        public static ReactiveCommand<T> CreateAsyncObservable<T>(Func<object, IObservable<T>> executeAsync, IScheduler executeScheduler = null, IScheduler scheduler = null)
        {
            return new ReactiveCommand<T>(Observable.Return(true), o => Observable.Start(() => executeAsync(o), executeScheduler ?? RxApp.TaskpoolScheduler).Switch(), scheduler);
        }

        public static ReactiveCommand<T> CreateAsyncTask<T>(IObservable<bool> canExecute, Func<object, CancellationToken, Task<T>> executeAsync, IScheduler executeScheduler = null, IScheduler scheduler = null)
        {
            return new ReactiveCommand<T>(canExecute, x => ObservableEx.StartAsync(ct => executeAsync(x, ct), executeScheduler ?? RxApp.TaskpoolScheduler), scheduler);
        }

        public static ReactiveCommand<T> CreateAsyncTask<T>(Func<object, CancellationToken, Task<T>> executeAsync, IScheduler executeScheduler = null, IScheduler scheduler = null)
        {
            return new ReactiveCommand<T>(Observable.Return(true), x => ObservableEx.StartAsync(ct => executeAsync(x, ct), executeScheduler ?? RxApp.TaskpoolScheduler), scheduler);
        }

        public static ReactiveCommand<Unit> CreateAsyncTask(IObservable<bool> canExecute, Func<object, CancellationToken, Task> executeAsync, IScheduler executeScheduler = null, IScheduler scheduler = null)
        {
            return new ReactiveCommand<Unit>(canExecute, x => ObservableEx.StartAsync(ct => executeAsync(x, ct), executeScheduler ?? RxApp.TaskpoolScheduler), scheduler);
        }

        public static ReactiveCommand<Unit> CreateAsyncTask(Func<object, CancellationToken, Task> executeAsync, IScheduler executeScheduler = null, IScheduler scheduler = null)
        {
            return new ReactiveCommand<Unit>(Observable.Return(true), x => ObservableEx.StartAsync(ct => executeAsync(x, ct), executeScheduler ?? RxApp.TaskpoolScheduler), scheduler);
        }
    }
}
