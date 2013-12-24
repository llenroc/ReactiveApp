using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;
using ReactiveApp.Services;

namespace ReactiveApp.Xaml
{
    public static class ISuspensionServiceExtensions
    {
        public static void SetupStartup(this ISuspensionService This, ReactiveApplication app)
        {
            Observable.Merge(This.IsLaunchingNew, This.IsResuming, This.IsUnpausing).SelectMany(args => app.View(args)).Subscribe();
        }
    }
}
