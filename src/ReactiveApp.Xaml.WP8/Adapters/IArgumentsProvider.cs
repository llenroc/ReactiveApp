using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Xaml.Adapters
{
    public interface IArgumentsProvider
    {
        IObservable<string> Arguments { get; }
    }
}
