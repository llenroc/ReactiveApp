using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Xaml.Services
{
    public class WinRTViewDispatcher : IViewDispatcher
    {
        private readonly IMainThreadDispatcher dispatcher;
        private readonly IViewPresenter viewPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinRTViewDispatcher"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="presenter">The presenter.</param>
        public WinRTViewDispatcher(IMainThreadDispatcher dispatcher, IViewPresenter viewPresenter)
        {
            this.dispatcher = dispatcher;
            this.viewPresenter = viewPresenter;
        }

        public IObservable<bool> CloseViewModel(IReactiveViewModel viewModel)
        {
            return this.dispatcher.RunOnMainThread<bool>(() => viewPresenter.Close(viewModel));
        }

        public IObservable<bool> OpenViewModel(ReactiveViewModelRequest viewModel)
        {
            return this.dispatcher.RunOnMainThread<bool>(() => viewPresenter.Open(viewModel));
        }
    }
}
