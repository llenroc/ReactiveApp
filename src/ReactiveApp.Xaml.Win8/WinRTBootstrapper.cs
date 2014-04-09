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
using Splat;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace ReactiveApp.Xaml
{
    public abstract class WinRTBootstrapper : ReactiveBootstrapper
    {
        private ISubject<LaunchActivatedEventArgs> launched;

        protected WinRTBootstrapper(ISubject<LaunchActivatedEventArgs> launched)
        {
            Contract.Requires<ArgumentNullException>(launched != null, "launched");

            this.launched = launched;
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
            return new SuspensionService(Application.Current, this.launched);
        }

        protected virtual void InitializeSuspensionService()
        {
            var suspensionService = CreateSuspensionService();
            Locator.CurrentMutable.RegisterConstant<ISuspensionService>(suspensionService);
        }

        protected override IMainThreadDispatcher CreateMainThreadDispatcher()
        {
            return new WinRTMainThreadDispatcher();
        }
    }
}
