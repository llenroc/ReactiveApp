using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using MonoTouch.UIKit;
using ReactiveApp.App;
using ReactiveApp.Exceptions;
using ReactiveApp.iOS.Services;
using ReactiveApp.Services;
using ReactiveUI;
using ReactiveUI.Mobile;
using Splat;

namespace ReactiveApp.iOS
{
    public abstract class iOSBootstrapper : ReactiveBootstrapper
    {
        private readonly ReactiveApplicationDelegate application;
        private readonly UIWindow window;
        private readonly AutoSuspendHelper suspendHelper;

        protected iOSBootstrapper(ReactiveApplicationDelegate application, UIWindow window, AutoSuspendHelper suspendHelper)
        {
            Contract.Requires<ArgumentNullException>(application != null, "application");
            Contract.Requires<ArgumentNullException>(window != null, "window");
            Contract.Requires<ArgumentNullException>(suspendHelper != null, "suspendHelper");

            this.application = application;
            this.window = window;
            this.suspendHelper = suspendHelper;
        }

        public override void Run()
        {
            base.Run();

            IExceptionHandler handler = Locator.Current.GetService<IExceptionHandler>();
            if (handler != null)
            {
                handler.SetupErrorHandling();
            }
            ISuspensionHost suspension = Locator.Current.GetService<ISuspensionHost>();
            if (suspension != null)
            {
                suspension.SetupDefaultSuspendResume();
            }
        }

        protected override void InitializePlatformServices()
        {
            InitializeSuspensionHost();
            base.InitializePlatformServices();
        }

        protected virtual ISuspensionHost CreateSuspensionHost()
        {
            return RxApp.SuspensionHost;
        }

        protected virtual void InitializeSuspensionHost()
        {
            var suspensionHost = CreateSuspensionHost();
            Locator.CurrentMutable.RegisterConstant<ISuspensionHost>(suspensionHost);
        }

        protected override IMainThreadDispatcher CreateMainThreadDispatcher()
        {
            return new iOSMainThreadDispatcher();
        }
        protected override IViewDispatcher CreateViewDispatcher()
        {
            var dispatcher = Locator.Current.GetService<IMainThreadDispatcher>();
            var viewPresenter = this.CreateViewPresenter();
            return new iOSViewDispatcher(dispatcher, viewPresenter);
        }

        protected override IViewPresenter CreateViewPresenter()
        {
            this.InitializeViewModelRequestTranslator();
            var requestTranslator = Locator.Current.GetService<IiOSViewModelRequestTranslator>();
            return new iOSViewPresenter(this.application, this.window, requestTranslator);
        }

        protected virtual void InitializeViewModelRequestTranslator()
        {
            var requestTranslator = this.CreateViewModelRequestTranslator();
            Locator.CurrentMutable.RegisterConstant<IiOSViewModelRequestTranslator>(requestTranslator);
        }

        protected virtual IiOSViewModelRequestTranslator CreateViewModelRequestTranslator()
        {
            var viewLocator = Locator.Current.GetService<ReactiveApp.Services.IViewLocator>();
            return new iOSViewModelRequestTranslator(viewLocator);
        }

    }
}