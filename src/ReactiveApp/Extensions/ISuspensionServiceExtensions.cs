using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.App;
using ReactiveApp.Services;
using ReactiveUI;
using Splat;

namespace ReactiveApp
{
    public static class ISuspensionServiceExtensions
    {
        public static void SetupStartup(this ISuspensionService This, IStartup startup = null)
        {
            IStartup start = startup ?? Locator.Current.GetService<IStartup>();
            if (start != null)
            {
                Observable.Merge(This.IsLaunchingNew, This.IsResuming, This.IsUnpausing).SelectMany(args => startup.Start(args).FirstOrDefaultAsync()).Subscribe(b =>
                {
                    if(!b)
                    {
                        throw new Exception("Startup failed");
                    }
                },
                ex => RxApp.DefaultExceptionHandler.OnNext(ex));
            }            
        }
    }
}
