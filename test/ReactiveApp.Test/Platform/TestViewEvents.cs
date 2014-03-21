using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Platform;

namespace ReactiveApp.Test.Platform
{
    class TestViewEvents : IViewEvents
    {
        private IObservable<object> firstLoaded;
        private IObservable<object> layoutUpdated;

        public TestViewEvents(IObservable<object> firstLoaded, IObservable<object> layoutUpdated)
        {
            this.firstLoaded = firstLoaded;
            this.layoutUpdated = layoutUpdated;
        }

        public IObservable<object> OnFirstLoaded(object view)
        {
            return this.firstLoaded;
        }

        public IObservable<object> OnLayoutUpdated(object view)
        {
            return this.layoutUpdated;
        }
    }
}
