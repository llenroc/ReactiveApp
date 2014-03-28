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
using ReactiveApp.Services;
#else
using System.Windows;
#endif

namespace ReactiveApp.Xaml
{
    public static class ApplicationExtensions
    {
        public static void SetupErrorHandling(this IExceptionHandler This, Application app = null)
        {
            Application application = app ?? Application.Current;

            if (application != null)
            {
#if !WINDOWS_PHONE
                var unhandled = Observable.FromEventPattern<UnhandledExceptionEventHandler, UnhandledExceptionEventArgs>(x => application.UnhandledException += x, x => application.UnhandledException -= x)
                    .Do(ue => ue.EventArgs.Handled = true)
                    .Select(ue => ue.EventArgs.Exception);
#else
                var unhandled = Observable.FromEventPattern<ApplicationUnhandledExceptionEventArgs>(x => application.UnhandledException += x, x => application.UnhandledException -= x)
                    .Do(ue => ue.EventArgs.Handled = true)
                    .Select(ue => ue.EventArgs.ExceptionObject);
#endif

                var unobserved = Observable.FromEventPattern<UnobservedTaskExceptionEventArgs>(x => TaskScheduler.UnobservedTaskException += x, x => TaskScheduler.UnobservedTaskException -= x)
                    .Do(ue => ue.EventArgs.SetObserved())
                    .Select(ue => ue.EventArgs.Exception);

                unhandled.Merge(unobserved).Subscribe(This.HandleException);
            }
        }
    }
}
