using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
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

        protected iOSBootstrapper(ReactiveApplicationDelegate application)
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
    }
}