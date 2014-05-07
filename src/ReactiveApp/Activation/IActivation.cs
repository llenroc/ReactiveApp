using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Activation
{
    public interface IActivation
    {
        IObservable<Tuple<IDataContainer, IDataContainer>> Activated { get; }

        IObservable<Unit> Deactivated { get; }
    }
}
