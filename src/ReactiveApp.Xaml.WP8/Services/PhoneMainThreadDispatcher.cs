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

namespace ReactiveApp.Xaml.Services
{
    public class PhoneMainThreadDispatcher : MainThreadDispatcher
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneMainThreadDispatcher"/> class.
        /// </summary>
        public PhoneMainThreadDispatcher()
            : base(RxApp.MainThreadScheduler)
        { }
    }
}
