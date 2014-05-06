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
        public static void WhenActivatedWithState(this IReactiveActivatableViewModel This, Func<IDataContainer, IDataContainer, IEnumerable<IDisposable>> activationBlock)
        {
            This.ReactiveActivator.AddActivationBlock((param, state) => activationBlock(param, state));
        }

        public static void WhenActivatedWithState(this IReactiveActivatableViewModel This, Action<IDataContainer, IDataContainer, Action<IDisposable>> activationBlock)
        {
            This.ReactiveActivator.AddActivationBlock((param, state) =>
            {
                var ret = new List<IDisposable>();
                activationBlock(param, state, ret.Add);
                return ret;
            });
        }
    }
}
