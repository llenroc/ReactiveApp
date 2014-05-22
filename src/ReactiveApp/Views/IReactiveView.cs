using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Activation;
using ReactiveUI;

namespace ReactiveApp.Views
{
    public interface IReactiveView : IViewFor, IReactiveActivatable, IActivation
    {
    }

    public interface IReactiveView<T> : IReactiveView, IViewFor<T> where T : class
    {
    }
}
