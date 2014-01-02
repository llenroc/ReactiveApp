using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Exceptions;
using ReactiveApp.Interfaces;
using Splat;

#if !WINDOWS_PHONE
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace ReactiveApp.Xaml
{
    public static class ReactiveApplicationExtensions
    {
        public static void SetupStartup(this ReactiveApplication This)
        {
            This.SuspensionService.SetupStartup(This);
        }

        public static void SetupErrorHandling(this ReactiveApplication This, IExceptionHandler handler = null)
        {
            IExceptionHandler exceptionHandler = handler ?? Locator.Current.GetService<IExceptionHandler>();

            if (exceptionHandler != null)
            {
#if !WINDOWS_PHONE
                var unhandled = Observable.FromEventPattern<UnhandledExceptionEventHandler, UnhandledExceptionEventArgs>(x => This.UnhandledException += x, x => This.UnhandledException -= x)
                    .Do(ue => ue.EventArgs.Handled = true)
                    .Select(ue => ue.EventArgs.Exception);
#else
                var unhandled = Observable.FromEventPattern<ApplicationUnhandledExceptionEventArgs>(x => This.UnhandledException += x, x => This.UnhandledException -= x)
                    .Do(ue => ue.EventArgs.Handled = true)
                    .Select(ue => ue.EventArgs.ExceptionObject);
#endif

                var unobserved = Observable.FromEventPattern<UnobservedTaskExceptionEventArgs>(x => TaskScheduler.UnobservedTaskException += x, x => TaskScheduler.UnobservedTaskException -= x)
                    .Do(ue => ue.EventArgs.SetObserved())
                    .Select(ue => ue.EventArgs.Exception);

                unhandled.Merge(unobserved).Subscribe(exceptionHandler.HandleException);
            }
        }
    }
}
