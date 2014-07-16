using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Exceptions;
using ReactiveApp.Services;
using ReactiveApp.Xaml.Services;
using ReactiveUI;
using Splat;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ReactiveApp.Xaml
{
    public abstract class WinRTBootstrapper : ReactiveBootstrapper
    {
        private readonly Frame frame;
        private readonly AutoSuspendHelper suspendHelper;

        protected WinRTBootstrapper(Frame frame, AutoSuspendHelper suspendHelper)
        {
            Contract.Requires<ArgumentNullException>(frame != null, "frame");
            Contract.Requires<ArgumentNullException>(suspendHelper != null, "suspendHelper");

            this.frame = frame;
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
            InitializeOrientationManager();
            base.InitializePlatformServices();
        }

        protected virtual Func<object> CreateNewAppStateFunction()
        {
            return () => new object();
        }

        protected virtual ISuspensionHost CreateSuspensionHost()
        {
            RxApp.SuspensionHost.CreateNewAppState = CreateNewAppStateFunction();
            return RxApp.SuspensionHost;
        }

        protected virtual void InitializeSuspensionHost()
        {
            var suspensionHost = CreateSuspensionHost();
            Locator.CurrentMutable.RegisterConstant<ISuspensionHost>(suspensionHost);
        }

        protected virtual IOrientationManager CreateOrientationManager()
        {
            var orientationManager = OrientationManager.Instance;
            return orientationManager;
        }

        protected virtual void InitializeOrientationManager()
        {
            var orientationManager = CreateOrientationManager();
            Locator.CurrentMutable.RegisterConstant<IOrientationManager>(orientationManager);
        }

        protected override IMainThreadDispatcher CreateMainThreadDispatcher()
        {
            return new WinRTMainThreadDispatcher();
        }

        protected override IViewDispatcher CreateViewDispatcher()
        {
            var dispatcher = Locator.Current.GetService<IMainThreadDispatcher>();
            var viewPresenter = Locator.Current.GetService<IViewPresenter>();
            return new WinRTViewDispatcher(dispatcher, viewPresenter);
        }

        protected override IViewPresenter CreateViewPresenter()
        {
            var viewLocator = Locator.Current.GetService<ReactiveApp.Services.IViewLocator>();
            return new WinRTViewPresenter(this.frame, viewLocator);
        }
    }
}
