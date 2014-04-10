using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Services
{
    public interface IViewPresenter
    {
        IObservable<Unit> Open(ReactiveViewModelRequest viewModel);

        IObservable<Unit> Close();
    }
}
