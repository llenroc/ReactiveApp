using System;

namespace ReactiveApp.Navigation
{
    public interface IReactiveAppBarManager
    {
        IDisposable AddAppBar(ReactiveAppBar appbar);
    }
}
