using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using Splat;

namespace ReactiveApp.App
{
    public class Startup<TViewModel> : ReactiveViewModel, IStartup
        where TViewModel : IReactiveViewModel
    {
        public IObservable<Unit> Start(object hint = null)
        {
            if (hint != null)
            {
                this.Log().Info("Hint ignored in default Startup: {0}", hint.ToString());
            }
            //TODO
            return null;
        }
    }
}
