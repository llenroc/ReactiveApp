using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;

namespace ReactiveApp
{
    public static class IReactiveViewModelExtensions
    {
        public static IObservable<Unit> RunOnMainThread(this IReactiveViewModel This, Action action)
        {
            return This.MainThread.RunOnMainThread(action);
        }

        public static IObservable<T> RunOnMainThread<T>(this IReactiveViewModel This, Func<T> func)
        {
            return This.MainThread.RunOnMainThread(func);
        }

        public static IObservable<Unit> RunOnMainThread(this IReactiveViewModel This, Func<CancellationToken, Task> task)
        {
            return This.MainThread.RunOnMainThread(task);
        }

        public static IObservable<T> RunOnMainThread<T>(this IReactiveViewModel This, Func<CancellationToken, Task<T>> task)
        {
            return This.MainThread.RunOnMainThread(task);
        }

        public static IObservable<T> RunOnMainThread<T>(this IReactiveViewModel This, Func<IObservable<T>> func)
        {
            return This.RunOnMainThread<IObservable<T>>(func).Merge();
        }

        public static IObservable<bool> CloseViewModel(this IReactiveViewModel This)
        {
            return This.ViewDispatcher.CloseViewModel(This);
        }

        public static IObservable<bool> CloseViewModel(this IReactiveViewModel This, IReactiveViewModel viewModel)
        {
            return This.ViewDispatcher.CloseViewModel(viewModel);
        }

        public static IObservable<bool> OpenViewModel<TViewModel>(this IReactiveViewModel This) where TViewModel : IReactiveViewModel
        {
            return This.OpenViewModel(typeof(TViewModel), null, null);
        }

        public static IObservable<bool> OpenViewModel<TViewModel>(this IReactiveViewModel This, 
            IDictionary<string, string> parameters, IDictionary<string, string> presentationInfo = null) where TViewModel : IReactiveViewModel
        {
            return This.OpenViewModel( typeof(TViewModel), new DataContainer(parameters), new DataContainer(presentationInfo));
        }

        public static IObservable<bool> OpenViewModel<TViewModel>(this IReactiveViewModel This,
            IDataContainer parameters, IDataContainer presentationInfo = null) where TViewModel : IReactiveViewModel
        {
            return This.OpenViewModel(typeof(TViewModel), parameters, presentationInfo);
        }

        public static IObservable<bool> OpenViewModel(this IReactiveViewModel This, 
            Type viewModelType, IDataContainer parameters, IDataContainer presentationInfo)
        {
            return This.ViewDispatcher.OpenViewModel(new ReactiveViewModelRequest(viewModelType, parameters, presentationInfo));
        }
    }
}
