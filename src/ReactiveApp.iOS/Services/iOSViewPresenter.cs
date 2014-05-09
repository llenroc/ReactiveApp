using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.iOS.Views;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;
using ReactiveUI;
using Splat;

namespace ReactiveApp.iOS.Services
{
    public class iOSViewPresenter : IViewPresenter
    {
        private readonly UIApplicationDelegate applicationDelegate;
        private readonly UIWindow window;
        private readonly IiOSViewModelRequestTranslator translator;

        public iOSViewPresenter(UIApplicationDelegate applicationDelegate, UIWindow window, IiOSViewModelRequestTranslator translator)
        {
            this.applicationDelegate = applicationDelegate;
            this.window = window;
            this.translator = translator;
        }

        public IObservable<bool> Open(ReactiveViewModelRequest viewModel)
        {
            IiOSReactiveView view = this.translator.GetViewControllerForViewModelRequest(viewModel);

            var viewController = view as UIViewController;
            if (viewController == null)
            {
                throw new InvalidOperationException("Passed in IiOSReactiveView is not a UIViewController");
            }

            if (this.MasterNavigationController == null)
            {
                this.CreateMasterNavigationController(viewController);
            }
            else
            {
                this.MasterNavigationController.PushViewController(viewController, true);
            }

            return Observable.Return(true);
        }

        public IObservable<bool> Close(IReactiveViewModel viewModel)
        {
            var topViewController = this.MasterNavigationController.TopViewController;

            if (topViewController == null)
            {
                this.Log().Info("No topmost view controller found. Ignore close for viewmodel {0}.", viewModel);
                return Observable.Return(false);
            }

            IViewFor currentView = topViewController as IViewFor;
            if (currentView == null)
            {
                this.Log().Info("Topmost view controller is not IViewFor. Ignore close for viewmodel {0}.", viewModel);
                return Observable.Return(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                this.Log().Info("Topmost view controller does not correspond to the closing viewmodel. Ignore close for viewmodel {0}.", viewModel);
                return Observable.Return(false);
            }

            this.MasterNavigationController.PopViewControllerAnimated(true);
            return Observable.Return(true);
        }

        protected virtual void CreateMasterNavigationController(UIViewController viewController)
        {
            foreach (var view in window.Subviews)
            {
                view.RemoveFromSuperview();
            }

            this.MasterNavigationController = CreateNavigationController(viewController);

            this.SetWindowRootViewController(MasterNavigationController);
        }

        protected virtual UINavigationController CreateNavigationController(UIViewController viewController)
        {
            return new UINavigationController(viewController);
        }
        protected virtual void SetWindowRootViewController(UIViewController controller)
        {
            this.window.AddSubview(controller.View);
            this.window.RootViewController = controller;
        }
        
        public UINavigationController MasterNavigationController { get; private set; }
    }
}