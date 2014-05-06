using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using ReactiveUI;

namespace ReactiveApp.Activation
{
    public interface IReactiveActivationForViewFetcher : IActivationForViewFetcher
    {
        Tuple<IObservable<Tuple<IDataContainer, IDataContainer>>, IObservable<Unit>> GetActivationForView(IReactiveActivatable view);
    }
}
