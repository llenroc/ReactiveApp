using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;
using ReactiveApp.Xaml.Controls;
using ReactiveUI;

namespace ReactiveApp.Xaml
{
    public abstract class ReactiveShellApplication : ReactiveApplication, IApplication<ReactiveShell, ReactiveView> 
    {
        public ReactiveShellApplication()
        {
            this.Log().Info("Creating Shell.");
            this.Shell = this.CreateShell();
        }

        protected abstract ReactiveShell CreateShell();

        protected override IObservable<Unit> Activate()
        {
            this.Log().Info("Activating Shell.");
            return this.Shell.Activate();
        }

        /// <summary>
        /// Gets the shell.
        /// </summary>
        /// <value>
        /// The shell.
        /// </value>
        public ReactiveShell Shell { get; private set; }
    }
}
