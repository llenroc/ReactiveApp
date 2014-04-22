using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;
using ReactiveUI;
using Windows.UI.Xaml.Controls;
using Splat;

namespace ReactiveApp.Xaml.Services
{
    public class WinRTViewPresenter : IViewPresenter
    {
        private readonly Frame frame;
        private readonly ReactiveApp.Services.IViewLocator viewLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="WinRTViewPresenter" /> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="viewLocator">The request translator.</param>
        public WinRTViewPresenter(Frame frame, ReactiveApp.Services.IViewLocator viewLocator)
        {
            this.frame = frame;
            this.viewLocator = viewLocator;
        }

        public virtual IObservable<bool> Open(ReactiveViewModelRequest viewModelRequest)
        {
            this.Log().Info("Navigating to {0}", viewModelRequest.ViewModelType);

            try
            {
                Type viewType = this.viewLocator.GetViewTypeForViewModel(viewModelRequest.ViewModelType);
                bool success = this.frame.Navigate(viewType, viewModelRequest);
                return Observable.Return(success);
            }
            catch (Exception exception)
            {
                this.Log().ErrorException("Error occurred during navigation.", exception);
                return Observable.Return(false);
            }
        }

        public virtual IObservable<bool> Close(IReactiveViewModel viewModel)
        {
            IViewFor currentView = this.frame.Content as IViewFor;
            if (currentView == null)
            {
                this.Log().Info("Frame has no page. Ignore close for viewmodel {0}.", viewModel);
                return Observable.Return(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                this.Log().Info("The current page does not correspond to the closing viewmodel. Ignore close for viewmodel {0}.", viewModel);
                return Observable.Return(false);
            }

            if (!frame.CanGoBack)
            {
                this.Log().Info("The frame can not go back. Ignore close for viewmodel {0}.", viewModel);
                return Observable.Return(false);
            }

            this.Log().Info("Closing {0}", viewModel);

            this.frame.GoBack();
            return Observable.Return(true);
        }
    }
}
