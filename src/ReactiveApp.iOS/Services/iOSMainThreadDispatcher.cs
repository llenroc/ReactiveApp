using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveUI;

namespace ReactiveApp.iOS.Services
{
    public class iOSMainThreadDispatcher : MainThreadDispatcher
    {
        public iOSMainThreadDispatcher()
            : base(RxApp.MainThreadScheduler)
        { }
    }
}
