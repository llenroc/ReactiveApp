using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveApp.Platform
{
    public interface IViewEvents
    {
        IObservable<object> OnFirstLoaded(object view);

        IObservable<object> OnLayoutUpdated(object view);
    }
}
