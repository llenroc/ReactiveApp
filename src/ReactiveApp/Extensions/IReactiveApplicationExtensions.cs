using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.App;
using ReactiveApp.ViewModels;

namespace ReactiveApp
{
    public static class IReactiveApplicationExtensions
    {
        public static void RegisterAppStart<TViewModel>(this IReactiveApplication This)
            where TViewModel : IReactiveViewModel
        {
            This.RegisterStartup(new Startup<TViewModel>());
        }
    }
}
