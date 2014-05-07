using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using ReactiveUI;

namespace ReactiveApp.Activation
{
    public class ActivationViewFetcher : IReactiveActivationForViewFetcher
    {
        /// <summary>
        /// The affinity for types implementing IActivation to be chosen for activation.
        /// The value for IActivationViewFetcher is 10 so we need to make sure this class has higher affinity.
        /// </summary>
        private const int Affinity = 20;

        public Tuple<IObservable<Unit>, IObservable<Unit>> GetActivationForView(IActivatable view)
        {
            var ca = view as IActivation;
            return Tuple.Create(ca.Activated.Select(_ => Unit.Default), ca.Deactivated);
        }

        public Tuple<IObservable<Tuple<IDataContainer, IDataContainer>>, IObservable<Unit>> GetActivationForView(IReactiveActivatable view)
        {
            var ca = view as IActivation;
            return Tuple.Create(ca.Activated, ca.Deactivated);
        }

        public int GetAffinityForView(Type view)
        {
            return (typeof(IActivation).GetTypeInfo().IsAssignableFrom(view.GetTypeInfo())) ?
                Affinity : 0;
        }
    }
}
