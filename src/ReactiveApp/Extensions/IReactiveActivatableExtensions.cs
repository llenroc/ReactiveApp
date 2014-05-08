﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Activation;
using ReactiveApp.ViewModels;
using ReactiveUI;
using Splat;

namespace ReactiveApp
{
    public static class IReactiveActivatableExtensions
    {
        public static IDisposable WhenActivatedWithState(this IReactiveActivatable This, Func<IDataContainer, IDataContainer, IEnumerable<IDisposable>> activationBlock)
        {
            var activationFetcher = activationFetcherCache.Get(This.GetType());
            if (activationFetcher == null)
            {
                var msg = "Don't know how to detect when {0} is activated/deactivated, you may need to implement IReactiveActivationForViewFetcher";
                throw new ArgumentException(String.Format(msg, This.GetType().FullName));
            }

            var activationEvents = activationFetcher.GetActivationForView(This);

            var vmDisposable = Disposable.Empty;
            var iViewFor = This as IViewFor;
            if (iViewFor != null)
            {
                vmDisposable = handleViewModelActivation(iViewFor, activationEvents.Item1, activationEvents.Item2);
            }

            var viewDisposable = handleViewActivation(activationBlock, activationEvents.Item1, activationEvents.Item2);
            return new CompositeDisposable(vmDisposable, viewDisposable);
        }

        public static IDisposable WhenActivatedWithState(this IReactiveActivatable This, Action<IDataContainer, IDataContainer, Action<IDisposable>> activationBlock)
        {
            return This.WhenActivatedWithState((param, state) =>
            {
                var ret = new List<IDisposable>();
                activationBlock(param, state, ret.Add);
                return ret;
            });
        }

        static IDisposable handleViewActivation(Func<IDataContainer, IDataContainer, IEnumerable<IDisposable>> activationBlock, IObservable<Tuple<IDataContainer, IDataContainer>> activation, IObservable<Unit> deactivation)
        {
            var viewDisposable = new SerialDisposable();

            return new CompositeDisposable(
                // Activation
                activation.Subscribe(tuple =>
                {
                    // NB: We need to make sure to respect ordering so that the cleanup
                    // happens before we invoke block again
                    viewDisposable.Disposable = Disposable.Empty;
                    viewDisposable.Disposable = new CompositeDisposable(activationBlock(tuple.Item1, tuple.Item2));
                }),
                // Deactivation
                deactivation.Subscribe(_ =>
                {
                    viewDisposable.Disposable = Disposable.Empty;
                }),
                viewDisposable);
        }

        static IDisposable handleViewModelActivation(IViewFor view, IObservable<Tuple<IDataContainer, IDataContainer>> activation, IObservable<Unit> deactivation)
        {
            var vmDisposable = new SerialDisposable();

            return new CompositeDisposable(
                // Activation
                activation.Select(tuple => view.WhenAnyValue(x => x.ViewModel).Select(x => Tuple.Create(x as IReactiveActivatableViewModel, tuple)))
                    .Switch()                    
                    .Subscribe(tuple =>
                    {
                        // NB: We need to make sure to respect ordering so that the cleanup
                        // happens before we activate again
                        vmDisposable.Disposable = Disposable.Empty;
                        if (tuple.Item1 != null)
                        {
                            vmDisposable.Disposable = tuple.Item1.ReactiveActivator.Activate(tuple.Item2.Item1, tuple.Item2.Item2);
                        }
                    }),
                // Deactivation
                deactivation.Subscribe(_ =>
                {
                    vmDisposable.Disposable = Disposable.Empty;
                }),
                vmDisposable);
        }


        private static readonly MemoizingMRUCache<Type, IReactiveActivationForViewFetcher> activationFetcherCache =
            new MemoizingMRUCache<Type, IReactiveActivationForViewFetcher>((t, _) =>
            {
                return Locator.Current.GetServices<IReactiveActivationForViewFetcher>()
                    .Aggregate(Tuple.Create(0, default(IReactiveActivationForViewFetcher)), (acc, x) =>
                {
                    int score = x.GetAffinityForView(t);
                    return (score > acc.Item1) ? Tuple.Create(score, x) : acc;
                }).Item2;
            }, RxApp.SmallCacheLimit);
    }
}