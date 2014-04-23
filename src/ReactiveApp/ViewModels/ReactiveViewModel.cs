using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Activation;
using ReactiveApp.Services;
using ReactiveUI;
using Splat;

namespace ReactiveApp.ViewModels
{
    public class ReactiveViewModel : ReactiveObject, IReactiveViewModel, IEnableLogger
    {
        public ReactiveViewModel()
        {
            this.Activator = new ViewModelActivator();
            this.ReactiveActivator = new ReactiveViewModelActivator(this);

            this.MainThread = Locator.Current.GetService<IMainThreadDispatcher>();
            this.ViewDispatcher = Locator.Current.GetService<IViewDispatcher>();
        }

        public ViewModelActivator Activator { get; private set; }

        public IViewModelActivator ReactiveActivator { get; private set; }

        public IMainThreadDispatcher MainThread { get; private set; }

        public IViewDispatcher ViewDispatcher { get; private set; }
    }
}
