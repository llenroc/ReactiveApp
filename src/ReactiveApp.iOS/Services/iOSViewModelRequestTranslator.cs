using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;

namespace ReactiveApp.iOS.Services
{
    public class iOSViewModelRequestTranslator : IiOSViewModelRequestTranslator
    {
        private readonly IViewLocator viewLocator;

        public iOSViewModelRequestTranslator(IViewLocator viewLocator)
        {
            this.viewLocator = viewLocator;
        }

        public UIViewController GetViewControllerForViewModelRequest(ReactiveViewModelRequest viewModelRequest)
        {
            var viewType = viewLocator.GetViewTypeForViewModel(viewModelRequest.ViewModelType);
            if (viewType == null)
            {
                throw new Exception("View Type not found for " + viewModelRequest.ViewModelType);
            }

            var view = CreateViewOfType(viewType, viewModelRequest);
            return view;
        }

        protected virtual UIViewController CreateViewOfType(Type viewType, ReactiveViewModelRequest viewModelRequest)
        {
            var view = Activator.CreateInstance(viewType) as UIViewController;
            if (view == null)
            {
                throw new Exception("View not loaded for " + viewType);
            }
            return view;
        }
    }
}