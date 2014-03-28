using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

using ReactiveApp.Interfaces;
using ReactiveApp.Services;
using ReactiveApp.Xaml.Controls;
using ReactiveApp.Xaml.Services;
using ReactiveUI;
using Splat;

#if !WINDOWS_PHONE
using Windows.UI.Xaml;
using Windows.ApplicationModel.Activation;
#else
using System.Windows;
using Microsoft.Phone.Shell;
#endif

namespace ReactiveApp.Xaml
{
    public abstract class ReactiveApplication : Application
    {
#if !WINDOWS_PHONE
        // an app can only launch once and we want to remember that value
        internal readonly ISubject<LaunchActivatedEventArgs> launched = new ReplaySubject<LaunchActivatedEventArgs>();
#else
        internal readonly IPhoneFrameHelper frameHelper;
#endif

        public ReactiveApplication()
        {
#if WINDOWS_PHONE
            this.frameHelper = new PhoneFrameHelper(this.Shell);
#endif

#if !WINDOWS_PHONE
            var suspensionService = new SuspensionService(this, launched);
#else
            this.ApplicationLifetimeObjects.Add(new PhoneApplicationService());
            var suspensionService = new SuspensionService(this, this.frameHelper);
#endif
            this.SuspensionService = suspensionService;  
          
            this.Configure();
        }

        protected abstract void Configure();

        public abstract IObservable<Unit> View(string args);
        
        protected virtual IObservable<Unit> Activate()
        {
#if !WINDOWS_PHONE
            SystemTrayManager.Initialize();
#else
            OrientationManager.Initialize(this.frameHelper);
            SystemTrayManager.Initialize(this.frameHelper);
#endif
#if !WINDOWS_PHONE
            if (Window.Current.Content != this.Shell)
            {
                Window.Current.Content = this.Shell;
            }
            Window.Current.Activate();
            return Observable.Return<Unit>(Unit.Default);
#else
            return this.frameHelper.Activate();
#endif
        }

#if !WINDOWS_PHONE
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            base.OnLaunched(args);
            this.launched.OnNext(args);
        }
#endif

        /// <summary>
        /// Gets the suspension service.
        /// </summary>
        /// <value>
        /// The suspension service.
        /// </value>
        public ISuspensionService SuspensionService { get; private set; }
    }
}
