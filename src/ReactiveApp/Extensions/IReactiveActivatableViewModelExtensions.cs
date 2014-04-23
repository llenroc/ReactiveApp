using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Activation;
using ReactiveApp.ViewModels;

namespace ReactiveApp
{
    public static class IReactiveActivatableViewModelExtensions
    {
        public static void WhenActivatedWithState(this IReactiveActivatableViewModel This, Func<IDataContainer, IDataContainer, IEnumerable<IDisposable>> activationBlock, Action<IDataContainer> deactivationBlock)
        {
            This.ReactiveActivator.AddActivationBlock((param, state) => 
                new IDisposable[] { Disposable.Create(() => deactivationBlock(state)) }
                .Concat(activationBlock(param, state)));
        }

        public static void WhenActivatedWithState(this IReactiveActivatableViewModel This, Action<IDataContainer, IDataContainer, Action<IDisposable>> activationBlock, Action<IDataContainer> deactivationBlock)
        {
            This.ReactiveActivator.AddActivationBlock((param, state) =>
            {
                var ret = new List<IDisposable>();
                ret.Add(Disposable.Create(() => deactivationBlock(state)));
                activationBlock(param, state, ret.Add);
                return ret;
            });
        }
    }
}
