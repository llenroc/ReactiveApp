using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Xaml.Services
{
    /// <summary>
    ///
    /// </summary>
    internal interface IPhoneFrameHelper
    {
        IObservable<string> Arguments { get; }

        IObservable<Unit> Activate();
    }
}
