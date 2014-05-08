using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.iOS.Views;
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

        public IiOSReactiveView GetViewControllerForViewModelRequest(ReactiveViewModelRequest viewModelRequest)
        {
            var viewType = this.viewLocator.GetViewTypeForViewModelType(viewModelRequest.ViewModelType);
            if (viewType == null)
            {
                throw new Exception("View Type not found for " + viewModelRequest.ViewModelType);
            }

            var view = this.CreateViewOfType(viewType);
            view.Request = viewModelRequest;
            return view;
        }

        protected virtual IiOSReactiveView CreateViewOfType(Type viewType)
        {
            var view = Activator.CreateInstance(viewType) as IiOSReactiveView;
            if (view == null)
            {
                throw new Exception("View not loaded for " + viewType);
            }
            return view;
        }
    }
}