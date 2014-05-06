using System;
using System.Collections.Generic;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Activation
{
    public interface IViewModelActivator
    {
        void AddActivationBlock(Func<IDataContainer, IDataContainer, IEnumerable<IDisposable>> block);

        IDisposable Activate(IDataContainer parameters, IDataContainer state);

        void Deactivate(bool ignoreRefCount = false);
    }
}