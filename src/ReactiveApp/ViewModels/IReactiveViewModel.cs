using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Activation;
using ReactiveApp.Services;

namespace ReactiveApp.ViewModels
{
    public interface IReactiveViewModel : IReactiveActivatableViewModel
    {
        IMainThreadDispatcher MainThread { get; }

        IViewDispatcher ViewDispatcher { get; }
    }
}
