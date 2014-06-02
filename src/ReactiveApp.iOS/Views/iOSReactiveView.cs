using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using ReactiveUI;
using ReactiveUI.Cocoa;

namespace ReactiveApp.iOS.Views
{
    public class iOSReactiveView : ReactivePageViewController, IiOSReactiveView
    {
        public iOSReactiveView()
        {
        }

        object IViewFor.ViewModel
        {
            get;
            set;
        }

        public ReactiveViewModelRequest Request { get; set; }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.ViewCreated(this.Request);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }
    }
}
