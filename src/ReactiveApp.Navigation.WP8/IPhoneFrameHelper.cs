using System;
using System.Reactive;
using Microsoft.Phone.Controls;

namespace ReactiveApp.Navigation
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
