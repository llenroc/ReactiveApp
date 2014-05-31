using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveUI;
using Splat;

namespace ReactiveApp.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ReactiveViewModel : ReactiveObject, IReactiveViewModel, IEnableLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveViewModel"/> class.
        /// </summary>
        public ReactiveViewModel()
        {
            this.Activator = new ViewModelActivator();

            this.MainThread = Locator.Current.GetService<IMainThreadDispatcher>();
            this.ViewDispatcher = Locator.Current.GetService<IViewDispatcher>();
        }

        public ViewModelActivator Activator { get; private set; }

        public IMainThreadDispatcher MainThread { get; private set; }

        public IViewDispatcher ViewDispatcher { get; private set; }
    }
}
