using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveApp
{
    public class ObservableEx
    {
        public static IObservable<Unit> StartAsync(Func<CancellationToken, Task> actionAsync, IScheduler scheduler)
        {
            var subject = new AsyncSubject<Unit>();
            scheduler.Schedule(() =>
            {
                var cancellable = new CancellationDisposable();

                var task = default(Task);
                try
                {
                    task = actionAsync(cancellable.Token);
                }
                catch (Exception exception)
                {
                    subject.OnError(exception);
                }

                var disp = task.ToObservable().Subscribe(subject);
            });
            return subject.AsObservable();
        }

        public static IObservable<TResult> StartAsync<TResult>(Func<CancellationToken, Task<TResult>> functionAsync, IScheduler scheduler)
        {
            var subject = new AsyncSubject<TResult>();
            scheduler.Schedule(() =>
            {
                var cancellable = new CancellationDisposable();

                var task = default(Task<TResult>);
                try
                {
                    task = functionAsync(cancellable.Token);
                }
                catch (Exception exception)
                {
                    subject.OnError(exception);
                }

                var disp = task.ToObservable().Subscribe(subject);
            });
            return subject.AsObservable();
        }
    }
}
