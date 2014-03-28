using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.App;
using ReactiveApp.Interfaces;
using ReactiveApp.Services;
using Splat;
using Windows.UI.Xaml;

namespace ReactiveApp.Xaml
{
    public static class ISuspensionServiceExtensions
    {
        public static void SetupStartup(this ISuspensionService This, IStartup startup = null)
        {
            IStartup start = startup ?? Locator.Current.GetService<IStartup>();
            if (start != null)
            {
                Observable.Merge(This.IsLaunchingNew, This.IsResuming, This.IsUnpausing).SelectMany(args => startup.Start(args).FirstOrDefaultAsync()).Subscribe();
            }            
        }
    }
}
