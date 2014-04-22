using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Exceptions;
using ReactiveApp.Services;
using Splat;

#if NETFX_CORE
using Windows.UI.Xaml;
#elif WINDOWS_PHONE
using System.Windows;
#endif

namespace ReactiveApp.Xaml
{
    public static class ApplicationExtensions
    {
        public static IDisposable SetupErrorHandling(this IExceptionHandler This, Application app = null)
        {
            Application application = app ?? Application.Current;

            if (application != null)
            {
#if NETFX_CORE
                var unhandled = Observable.FromEventPattern<UnhandledExceptionEventHandler, UnhandledExceptionEventArgs>(x => application.UnhandledException += x, x => application.UnhandledException -= x)
                    .Do(ue => ue.EventArgs.Handled = true)
                    .Select(ue => ue.EventArgs.Exception);
#elif WINDOWS_PHONE
                var unhandled = Observable.FromEventPattern<ApplicationUnhandledExceptionEventArgs>(x => application.UnhandledException += x, x => application.UnhandledException -= x)
                    .Do(ue => ue.EventArgs.Handled = true)
                    .Select(ue => ue.EventArgs.ExceptionObject);
#endif

                var unobserved = Observable.FromEventPattern<UnobservedTaskExceptionEventArgs>(x => TaskScheduler.UnobservedTaskException += x, x => TaskScheduler.UnobservedTaskException -= x)
                    .Do(ue => ue.EventArgs.SetObserved())
                    .Select(ue => ue.EventArgs.Exception);

                return unhandled.Merge(unobserved).Subscribe(This.HandleException);
            }
            else
            {
                return Disposable.Empty;
            }
        }
    }
}
