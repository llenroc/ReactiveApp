using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using ReactiveApp.Services;
using ReactiveUI;

namespace ReactiveApp.Android.Services
{
    public class AndroidMainThreadDispatcher : MainThreadDispatcher
    {
        public AndroidMainThreadDispatcher()
            : base(new SynchronizationContextScheduler(Application.SynchronizationContext))
        { }
    }
}
