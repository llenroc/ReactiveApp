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
using Splat;

namespace ReactiveApp.iOS
{
    public abstract class iOSBootstrapper : ReactiveBootstrapper
    {
        private readonly ReactiveApplicationDelegate application;
        private readonly UIWindow window;

        protected iOSBootstrapper(ReactiveApplicationDelegate application, UIWindow window)
        {
            Contract.Requires<ArgumentNullException>(application != null, "application");
            Contract.Requires<ArgumentNullException>(window != null, "window");

            this.application = application;
            this.window = window;
        }

        public override void Run()
        {
            base.Run();

            IExceptionHandler handler = Locator.Current.GetService<IExceptionHandler>();
            if (handler != null)
            {
                handler.SetupErrorHandling();
            }
            ISuspensionService suspension = Locator.Current.GetService<ISuspensionService>();
            if (suspension != null)
            {
                suspension.SetupStartup();
            }
        }

        protected override void InitializePlatformServices()
        {
            InitializeSuspensionService();
            base.InitializePlatformServices();
        }

        protected virtual ISuspensionService CreateSuspensionService()
        {
            return new SuspensionService(application);
        }

        protected virtual void InitializeSuspensionService()
        {
            var suspensionService = CreateSuspensionService();
            Locator.CurrentMutable.RegisterConstant<ISuspensionService>(suspensionService);
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

        protected virtual IViewPresenter CreateViewPresenter()
        {
            var requestTranslator = this.CreateViewModelRequestTranslator();
            return new iOSViewPresenter(this.application, this.window, requestTranslator);
        }

        protected virtual IiOSViewModelRequestTranslator CreateViewModelRequestTranslator()
        {
            var viewLocator = Locator.Current.GetService<ReactiveApp.Services.IViewLocator>();
            return new iOSViewModelRequestTranslator(viewLocator);
        }

    }
}