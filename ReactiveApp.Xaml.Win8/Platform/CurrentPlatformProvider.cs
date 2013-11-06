using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Platform;

namespace ReactiveApp.Xaml
{
    internal class CurrentPlatformProvider : IPlatformProvider
    {
        public IViewEvents ViewEvents
        {
            get { return XamlViewEvents.Instance; }
        }
    }
}
