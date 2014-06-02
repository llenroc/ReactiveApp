using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ReactiveApp.Exceptions;
using ReactiveApp.Services;
using ReactiveApp.Xaml.Adapters;
using ReactiveApp.Xaml.Services;
using ReactiveUI;
using ReactiveUI.Mobile;
using Splat;

namespace ReactiveApp.Xaml
{
    public abstract class PhoneBootstrapper : ReactiveBootstrapper
    {
        private readonly PhoneApplicationFrame frame;
        private readonly AutoSuspendHelper suspendHelper;

        protected PhoneBootstrapper(PhoneApplicationFrame frame, AutoSuspendHelper suspendHelper)
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
            base.InitializePlatformServices();

            InitializeSuspensionHost();
            InitializeOrientationManager();
            InitializeNavigationSerializer();
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
        protected virtual IOrientationManager CreateOrientationManager()
        {
            var orientationManager = OrientationManager.Instance;
            OrientationManager.InternalInstance.Initialize(this.frame);
            return orientationManager;
        }

        protected virtual void InitializeOrientationManager()
        {
            var orientationManager = CreateOrientationManager();
            Locator.CurrentMutable.RegisterConstant<IOrientationManager>(orientationManager);
        }

        protected virtual INavigationSerializer CreateNavigationSerializer()
        {
            //TODO: default implementation
            return null;
        }

        protected virtual void InitializeNavigationSerializer()
        {
            var serializer = CreateNavigationSerializer();
            Locator.CurrentMutable.RegisterConstant<INavigationSerializer>(serializer);
        }

        protected override IMainThreadDispatcher CreateMainThreadDispatcher()
        {
            return new PhoneMainThreadDispatcher();
        }

        protected override IViewDispatcher CreateViewDispatcher()
        {
            var dispatcher = Locator.Current.GetService<IMainThreadDispatcher>();
            var viewPresenter = this.CreateViewPresenter(this.frame);
            return new PhoneViewDispatcher(dispatcher, viewPresenter);
        }

        protected virtual IViewPresenter CreateViewPresenter(PhoneApplicationFrame frame)
        {
            var requestTranslator = this.CreateViewModelRequestTranslator();
            return new PhoneViewPresenter(frame, requestTranslator);
        }

        protected virtual IPhoneReactiveViewModelRequestTranslator CreateViewModelRequestTranslator()
        {
            var viewLocator = Locator.Current.GetService<ReactiveApp.Services.IViewLocator>();
            var navigationSerializer = Locator.Current.GetService<INavigationSerializer>();
            return new PhoneReactiveViewModelRequestTranslator(viewLocator, navigationSerializer);
        }
    }
}
