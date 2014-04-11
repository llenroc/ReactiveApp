using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using Splat;

namespace ReactiveApp.Services
{
    public interface IViewPresenter : IEnableLogger
    {
        IObservable<bool> Open(ReactiveViewModelRequest viewModel);

        IObservable<bool> Close(IReactiveViewModel viewModel);
    }
}
