using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;

namespace ReactiveApp.iOS.Services
{
    public class iOSViewDispatcher : IViewDispatcher
    {
        private readonly IMainThreadDispatcher dispatcher;
        private readonly IViewPresenter viewPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="iOSViewDispatcher"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="presenter">The presenter.</param>
        public iOSViewDispatcher(IMainThreadDispatcher dispatcher, IViewPresenter viewPresenter)
        {
            this.dispatcher = dispatcher;
            this.viewPresenter = viewPresenter;
        }

        public IObservable<bool> CloseViewModel(IReactiveViewModel viewModel)
        {
            return dispatcher.RunOnMainThread<bool>(() => viewPresenter.Close(viewModel));
        }

        public IObservable<bool> OpenViewModel(ReactiveViewModelRequest viewModel)
        {
            return dispatcher.RunOnMainThread<bool>(() => viewPresenter.Open(viewModel));
        }
    }
}
