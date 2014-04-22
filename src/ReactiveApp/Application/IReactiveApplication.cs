using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.App
{
    public interface IReactiveApplication
    {
        void Initialize();

        void RegisterStartup(IStartup startup);
    }
}
