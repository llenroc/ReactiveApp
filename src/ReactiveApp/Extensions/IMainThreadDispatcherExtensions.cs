using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;

namespace ReactiveApp
{
    public static class IMainThreadDispatcherExtensions
    {
        public static IObservable<T> RunOnMainThread<T>(this IMainThreadDispatcher dispatcher, Func<IObservable<T>> func)
        {
            return dispatcher.RunOnMainThread<IObservable<T>>(func).Switch();
        }
    }
}
