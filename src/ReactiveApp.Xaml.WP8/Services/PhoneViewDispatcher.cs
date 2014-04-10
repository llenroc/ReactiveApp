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
    public class PhoneViewDispatcher : IViewDispatcher
    {
        private readonly IMainThreadDispatcher dispatcher;
        private readonly IViewPresenter viewPresenter;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneViewDispatcher"/> class.
        /// </summary>
        /// <param name="dispatcher">The dispatcher.</param>
        /// <param name="presenter">The presenter.</param>
        public PhoneViewDispatcher(IMainThreadDispatcher dispatcher, IViewPresenter viewPresenter)
        {
            this.dispatcher = dispatcher;
            this.viewPresenter = viewPresenter;
        }

        public IObservable<Unit> CloseViewModel()
        {
            return dispatcher.RunOnMainThread<Unit>(() => viewPresenter.Close());
        }

        public IObservable<Unit> OpenViewModel(ReactiveViewModelRequest viewModel)
        {
            return dispatcher.RunOnMainThread<Unit>(() => viewPresenter.Open(viewModel));
        }
    }
}
