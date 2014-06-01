using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using ReactiveUI;

namespace ReactiveApp.iOS.Views
{
    public interface IiOSReactiveView : IReactiveView, ICanActivate
    {
        ReactiveViewModelRequest Request { get; set; }
    }
}