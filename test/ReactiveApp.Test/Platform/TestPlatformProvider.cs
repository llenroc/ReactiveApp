using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Platform;

namespace ReactiveApp.Test.Platform
{
    class TestPlatformProvider : IPlatformProvider
    {
        public IViewEvents ViewEvents { get; set; }
    }
}
