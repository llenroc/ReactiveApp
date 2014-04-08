using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveApp.Exceptions;

namespace ReactiveApp.Android
{
    public static class IExceptionHandlerExtensions
    {
        public static void SetupErrorHandling(this IExceptionHandler This, Application app)
        {
            Contract.Requires<ArgumentNullException>(app != null, "app");

            var unhandled = Observable.FromEventPattern<RaiseThrowableEventArgs>(x => AndroidEnvironment.UnhandledExceptionRaiser += x, x => AndroidEnvironment.UnhandledExceptionRaiser -= x)   
                .Do(ue => ue.EventArgs.Handled = true)
                .Select(ue => ue.EventArgs.Exception)
                .Merge(Observable.FromEventPattern<UnhandledExceptionEventHandler, UnhandledExceptionEventArgs>(x => AppDomain.CurrentDomain.UnhandledException += x, x => AppDomain.CurrentDomain.UnhandledException -= x)
                    .Select(ue => ue.EventArgs.ExceptionObject as Exception));

            var unobserved = Observable.FromEventPattern<UnobservedTaskExceptionEventArgs>(x => TaskScheduler.UnobservedTaskException += x, x => TaskScheduler.UnobservedTaskException -= x)
                .Do(ue => ue.EventArgs.SetObserved())
                .Select(ue => ue.EventArgs.Exception);

            unhandled.Merge(unobserved).Subscribe(This.HandleException);
        }
    }
}