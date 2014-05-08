using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.iOS.Views;
using ReactiveApp.ViewModels;

namespace ReactiveApp.iOS.Services
{
    public interface IiOSViewModelRequestTranslator
    {
        IiOSReactiveView GetViewControllerForViewModelRequest(ReactiveViewModelRequest viewModelRequest);
    }
}