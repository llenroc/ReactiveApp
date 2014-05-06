using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;

namespace ReactiveApp
{
    public static class IViewForExtensions
    {
        public static void ViewCreated(this IViewFor This, Func<object> viewModelFunc)
        {
            // fail fast if viewmodel already set
            if (This.ViewModel != null)
            {
                return;
            }

            var viewModel = viewModelFunc();
            if (viewModel == null)
            {
                LogHost.Default.Info("ViewModel not loaded for view {0}", This.GetType().Name);
                return;
            }

            This.ViewModel = viewModel;
        }
    }
}
