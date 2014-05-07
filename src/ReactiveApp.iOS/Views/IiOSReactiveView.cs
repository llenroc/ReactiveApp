using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;

namespace ReactiveApp.iOS.Views
{
    public interface IiOSReactiveView : IReactiveView
    {
        ReactiveViewModelRequest Request { get; set; }
    }
}