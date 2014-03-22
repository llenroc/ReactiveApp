using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Navigation
{
    public interface IReactiveAppBarManager
    {
        IDisposable AddAppBar(ReactiveAppBar appbar);
    }
}
