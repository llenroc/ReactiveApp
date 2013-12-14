using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Exceptions;
using ReactiveApp.Interfaces;
using ReactiveUI;
using Windows.UI.Xaml;

namespace ReactiveApp.Xaml
{
    public static class ReactiveApplicationExtensions
    {
        /// <summary>
        /// Setups the startup logic.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="This">The this.</param>
        public static void SetupStartup<T,U>(this ReactiveApplication<T,U> This)
            where T : class, IShell<T, U>
            where U : class, IView<T, U>
        {
            This.SuspensionService.SetupStartup(This);
        }

        /// <summary>
        /// Setups the error handling.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="This">The this.</param>
        /// <param name="handler">The handler.</param>
        public static void SetupErrorHandling<T,U>(this ReactiveApplication<T,U> This, IExceptionHandler handler = null)
            where T : class, IShell<T, U>
            where U : class, IView<T, U>
        {
            IExceptionHandler exceptionHandler = handler ?? RxApp.DependencyResolver.GetService<IExceptionHandler>();

            if (handler != null)
            {
                var unhandled = Observable.FromEventPattern<UnhandledExceptionEventHandler, UnhandledExceptionEventArgs>(x => This.UnhandledException += x, x => This.UnhandledException -= x)
                    .Do(ue => ue.EventArgs.Handled = true)
                    .Select(ue => ue.EventArgs.Exception);
                var unobserved = Observable.FromEventPattern<UnobservedTaskExceptionEventArgs>(x => TaskScheduler.UnobservedTaskException += x, x => TaskScheduler.UnobservedTaskException -= x)
                    .Do(ue => ue.EventArgs.SetObserved())
                    .Select(ue => ue.EventArgs.Exception);

                unhandled.Merge(unobserved).Subscribe(exceptionHandler.HandleException);
            }
        }
    }
}
