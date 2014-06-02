using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.App;
using ReactiveApp.Debugging;
using ReactiveApp.Exceptions;
using ReactiveApp.Services;
using ReactiveApp.Settings;
using ReactiveUI;
using Splat;

namespace ReactiveApp
{
    public abstract class ReactiveBootstrapper : IEnableLogger
    {
        protected abstract IReactiveApplication CreateApplication();

        protected abstract IMainThreadDispatcher CreateMainThreadDispatcher();

        protected abstract IViewDispatcher CreateViewDispatcher();

        public virtual void Run()
        {
            this.Log().Info("Initializing Dependency Resolver.");
            this.InitializeDependencyResolver();
            this.Log().Info("Before Bootstrapping.");
            this.BeforeBootstrapping();
            this.Log().Info("Initializing Debug Services.");
            this.InitializeDebugServices();
            this.Log().Info("Initializing Platform Services.");
            this.InitializePlatformServices();
            this.Log().Info("Initializing Settings.");
            this.InitializeSettings();
            this.Log().Info("Initializing MainThread Dispatcher.");
            this.InitializeMainThreadDispatcher();
            this.Log().Info("Initializing ViewModel Locator.");
            this.InitializeViewModelLocator();
            this.Log().Info("Initializing View Locator.");
            this.InitializeViewLocator();
            this.Log().Info("Initializing View Dispatcher.");
            this.InitializeViewDispatcher();
            this.Log().Info("Initializing Application.");
            this.InitializeApplication();
            this.Log().Info("After Bootstrapping.");
            this.AfterBootstrapping();
        }

        /// <summary>
        /// Befores the bootstrapping.
        /// </summary>
        protected virtual void BeforeBootstrapping()
        { }

        protected virtual void AfterBootstrapping()
        {
        }

        protected virtual void InitializeDebugServices()
        {
            this.InitializeObjectTracker();
        }

        protected virtual IObjectTracker CreateObjectTracker()
        {
            return ObjectTracker.Instance;
        }

        protected virtual void InitializeObjectTracker()
        {
            var objectTracker = this.CreateObjectTracker();
            Locator.CurrentMutable.RegisterConstant<IObjectTracker>(objectTracker);
        }
        
        protected virtual void InitializePlatformServices()
        {
            // do nothing by default
        }

        protected virtual IReactiveAppSettings CreateSettings()
        {
            return new ReactiveAppSettings();
        }

        protected virtual void InitializeSettings()
        {
            var settings = this.CreateSettings();
            Locator.CurrentMutable.RegisterConstant<IReactiveAppSettings>(settings);
        }

        protected virtual IExceptionHandler CreateExceptionHandler()
        {
            return new DefaultExceptionHandler();
        }

        protected virtual void InitializeExceptionHandler()
        {
            var exceptionHandler = this.CreateExceptionHandler();
            Locator.CurrentMutable.RegisterConstant<IExceptionHandler>(exceptionHandler);
        }

        protected virtual IViewModelLocator CreateViewModelLocator()
        {
            return new ViewModelLocator();
        }

        protected virtual void InitializeViewModelLocator()
        {
            var viewModelLocator = this.CreateViewModelLocator();
            Locator.CurrentMutable.RegisterConstant<IViewModelLocator>(viewModelLocator);
        }

        protected virtual ReactiveApp.Services.IViewLocator CreateViewLocator()
        {
            return new ReactiveApp.Services.ViewLocator();
        }

        protected virtual void InitializeViewLocator()
        {
            var viewLocator = this.CreateViewLocator();
            Locator.CurrentMutable.RegisterConstant<ReactiveApp.Services.IViewLocator>(viewLocator);
        }

        protected virtual void InitializeApplication()
        {
            var app = this.CreateApplication();
            app.Initialize();
            Locator.CurrentMutable.RegisterConstant<IReactiveApplication>(app);
        }

        protected virtual void InitializeMainThreadDispatcher()
        {
            var dispatcher = this.CreateMainThreadDispatcher();
            Locator.CurrentMutable.RegisterConstant<IMainThreadDispatcher>(dispatcher);
        }

        protected virtual void InitializeViewDispatcher()
        {
            var viewDispatcher = this.CreateViewDispatcher();
            Locator.CurrentMutable.RegisterConstant<IViewDispatcher>(viewDispatcher);
        }        

        protected virtual IDependencyResolver CreateDependencyResolver()
        {
            return Locator.Current;
        }

        protected virtual void InitializeDependencyResolver()
        {
            var resolver = this.CreateDependencyResolver();
            if(Locator.Current != resolver)
            {
                Locator.Current = resolver;
            }
            Locator.CurrentMutable.RegisterConstant<IDependencyResolver>(Locator.Current);
        }
    }
}
