using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;
using ReactiveUI;
using Splat;

namespace ReactiveApp.Xaml.Services
{
    public class PhoneViewPresenter : IViewPresenter
    {
        private readonly PhoneApplicationFrame frame;
        private readonly IPhoneReactiveViewModelRequestTranslator requestTranslator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneViewPresenter"/> class.
        /// </summary>
        /// <param name="frame">The frame.</param>
        /// <param name="requestTranslator">The request translator.</param>
        public PhoneViewPresenter(PhoneApplicationFrame frame, IPhoneReactiveViewModelRequestTranslator requestTranslator)
        {
            this.frame = frame;
            this.requestTranslator = requestTranslator;
        }

        public virtual IObservable<bool> Open(ReactiveViewModelRequest viewModel)
        {
            this.Log().Info("Navigating to {0}", viewModel.ViewModelType);

            try
            {
                var uri = requestTranslator.GetUriForViewModelRequest(viewModel);
                bool success = frame.Navigate(uri);
                return Observable.Return(success);
            }
            catch(Exception exception)
            {
                this.Log().ErrorException("Error occurred during navigation.", exception);
                return Observable.Return(false);
            }
        }

        public virtual IObservable<bool> Close(IReactiveViewModel viewModel)
        {
            IViewFor currentView = frame.Content as IViewFor;
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

            frame.GoBack();
            return Observable.Return(true);
        }
    }
}
