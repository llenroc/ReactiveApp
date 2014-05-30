using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveApp.Android.Services;
using ReactiveApp.App;
using ReactiveApp.Exceptions;
using ReactiveApp.Services;
using ReactiveUI;
using ReactiveUI.Mobile;
using Splat;

namespace ReactiveApp.Android
{
    public abstract class AndroidBootstrapper : ReactiveBootstrapper
    {
        private readonly Application application;

        protected AndroidBootstrapper(Application application)
        {
            Contract.Requires<ArgumentNullException>(application != null, "application");

            this.application = application;
        }

        public override void Run()
        {
            base.Run();

            IExceptionHandler handler = Locator.Current.GetService<IExceptionHandler>();
            if (handler != null)
            {
                handler.SetupErrorHandling(this.application);
            }
            ISuspensionHost suspension = Locator.Current.GetService<ISuspensionHost>();
            if (suspension != null)
            {
                suspension.SetupDefaultSuspendResume();
            }
        }

        protected override void InitializePlatformServices()
        {
            this.InitializeSuspensionHost();
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
            return new AndroidMainThreadDispatcher();
        }

        protected override IViewDispatcher CreateViewDispatcher()
        {
            var dispatcher = Locator.Current.GetService<IMainThreadDispatcher>();
            var viewPresenter = this.CreateViewPresenter();
            return new AndroidViewDispatcher(dispatcher, viewPresenter);
        }

        protected override IViewPresenter CreateViewPresenter()
        {
            this.InitializeViewModelRequestTranslator();
            var currentActivity = Locator.Current.GetService<IAndroidCurrentActivity>();
            var requestTranslator = Locator.Current.GetService<IAndroidReactiveViewModelRequestTranslator>();
            return new AndroidViewPresenter(currentActivity, requestTranslator);
        }

        protected virtual void InitializeViewModelRequestTranslator()
        {
            var requestTranslator = this.CreateViewModelRequestTranslator();
            Locator.CurrentMutable.RegisterConstant<IAndroidReactiveViewModelRequestTranslator>(requestTranslator);
        }

        protected virtual IAndroidReactiveViewModelRequestTranslator CreateViewModelRequestTranslator()
        {
            var viewLocator = Locator.Current.GetService<ReactiveApp.Services.IViewLocator>();
            var navigationSerializer = Locator.Current.GetService<INavigationSerializer>();
            return new AndroidReactiveViewModelRequestTranslator(this.application, viewLocator, navigationSerializer);
        }
    }
}