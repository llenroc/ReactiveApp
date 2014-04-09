using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Phone.Shell;
using ReactiveApp.Exceptions;
using ReactiveApp.Services;
using ReactiveApp.Xaml.Adapters;
using ReactiveApp.Xaml.Services;
using Splat;

namespace ReactiveApp.Xaml
{
    public abstract class PhoneBootstrapper : ReactiveBootstrapper
    {
        private readonly IArgumentsProvider arguments;
        private readonly IPhoneNavigationProvider navigation;

        protected PhoneBootstrapper(IPhoneNavigationProvider navigation, IArgumentsProvider arguments)
        {
            Contract.Requires<ArgumentNullException>(navigation != null, "navigation");
            Contract.Requires<ArgumentNullException>(arguments != null, "arguments");

            this.arguments = arguments;
            this.navigation = navigation;
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
            base.InitializePlatformServices();

            InitializeSuspensionService();
            InitializeOrientationManager();
        }

        protected virtual ISuspensionService CreateSuspensionService()
        {
            var phoneApplicationService = new PhoneApplicationService();
            Application.Current.ApplicationLifetimeObjects.Add(phoneApplicationService);
            return new SuspensionService(Application.Current, this.arguments);
        }

        protected virtual void InitializeSuspensionService()
        {
            var suspensionService = CreateSuspensionService();
            Locator.CurrentMutable.RegisterConstant<ISuspensionService>(suspensionService);
        }
        protected virtual IOrientationManager CreateOrientationManager()
        {
            var orientationManager = OrientationManager.Instance;
            OrientationManager.InternalInstance.Initialize(this.navigation.Frame);
            return orientationManager;
        }

        protected virtual void InitializeOrientationManager()
        {
            var orientationManager = CreateOrientationManager();
            Locator.CurrentMutable.RegisterConstant<IOrientationManager>(orientationManager);
        }

        protected override IMainThreadDispatcher CreateMainThreadDispatcher()
        {
            return new PhoneMainThreadDispatcher();
        }
    }
}
