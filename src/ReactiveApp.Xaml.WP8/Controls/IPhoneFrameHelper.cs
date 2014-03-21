using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;

namespace ReactiveApp.Xaml.Controls
{
    /// <summary>
    ///
    /// </summary>
    internal interface IPhoneFrameHelper
    {
        PhoneApplicationFrame Frame { get; }

        IObservable<string> Arguments { get; }

        IObservable<Unit> Activate();
    }
}
