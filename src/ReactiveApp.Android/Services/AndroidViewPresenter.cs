using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;
using ReactiveUI;
using Splat;

namespace ReactiveApp.Android.Services
{
    public class AndroidViewPresenter : IViewPresenter
    {
        private readonly IAndroidCurrentActivity currentActivity;
        private readonly IAndroidViewModelRequestTranslator requestTranslator;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidViewPresenter"/> class.
        /// </summary>
        /// <param name="currentActivity">The current activity.</param>
        /// <param name="requestTranslator">The request translator.</param>
        public AndroidViewPresenter(IAndroidCurrentActivity currentActivity, IAndroidViewModelRequestTranslator requestTranslator)
        {
            this.currentActivity = currentActivity;
            this.requestTranslator = requestTranslator;
        }

        public IObservable<bool> Open(ReactiveViewModelRequest viewModel)
        {
            Intent intent = requestTranslator.GetIntentForViewModelRequest(viewModel);

            return currentActivity.CurrentActivity.Select(act =>
            {
                act.StartActivity(intent);
                return true;
            });
        }

        public IObservable<bool> Close(IReactiveViewModel viewModel)
        {
            return currentActivity.CurrentActivity.Select(act =>
            {
                IViewFor currentView = act as IViewFor;
                if (currentView == null)
                {
                    this.Log().Info("Frame has no page. Ignore close for viewmodel {0}.", viewModel);
                    return false;
                }

                if (currentView.ViewModel != viewModel)
                {
                    this.Log().Info("The current page does not correspond to the closing viewmodel. Ignore close for viewmodel {0}.", viewModel);
                    return false;
                }

                this.Log().Info("Closing {0}", viewModel);

                act.Finish();
                return true;
            });
        }
    }
}