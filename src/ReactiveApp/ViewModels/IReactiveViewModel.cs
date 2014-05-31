using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveUI;

namespace ReactiveApp.ViewModels
{
    public interface IReactiveViewModel : ISupportsActivation
    {
        IMainThreadDispatcher MainThread { get; }

        IViewDispatcher ViewDispatcher { get; }
    }
}
