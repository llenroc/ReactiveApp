using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReactiveApp.Xaml.Controls;
using ReactiveApp.Interfaces;
using ReactiveUI;

#if !WINDOWS_PHONE
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace ReactiveApp.Xaml
{
    public abstract class ReactiveApplication<T,U> : Application
        where T : class, IShell<T, U>
        where U : class, IView<T, U>
    {
        public ReactiveApplication()
        {
            this.Shell = this.CreateShell();

            var resolver = this.CreateDependencyResolver();
            resolver.InitializeResolver();
            RxApp.DependencyResolver = resolver;

            this.Configure();
        }

        protected abstract void Configure();

        protected abstract IMutableDependencyResolver CreateDependencyResolver();

        protected abstract IShell<T, U> CreateShell();

        /// <summary>
        /// The root frame used for navigation.
        /// </summary>
        public IShell<T,U> Shell { get; private set; }
    }
}
