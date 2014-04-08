using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.Exceptions;

namespace ReactiveApp.iOS
{
    public static class IExceptionHandlerExtensions
    {
        public static void SetupErrorHandling(this IExceptionHandler This)
        {
            var unhandled = Observable.FromEventPattern<UnhandledExceptionEventHandler, UnhandledExceptionEventArgs>(x => AppDomain.CurrentDomain.UnhandledException += x, x => AppDomain.CurrentDomain.UnhandledException -= x)
                    .Select(ue => ue.EventArgs.ExceptionObject as Exception);

            var unobserved = Observable.FromEventPattern<UnobservedTaskExceptionEventArgs>(x => TaskScheduler.UnobservedTaskException += x, x => TaskScheduler.UnobservedTaskException -= x)
                .Do(ue => ue.EventArgs.SetObserved())
                .Select(ue => ue.EventArgs.Exception);

            unhandled.Merge(unobserved).Subscribe(This.HandleException);
        }
    }
}