using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using Splat;

namespace ReactiveApp
{
    public static class IReactiveViewExtensions
    {
        public static void ViewCreated(this IReactiveView This, ReactiveViewModelRequest viewModelRequest)
        {
            This.ViewCreated(() =>
            {
                var viewModel = Locator.Current.GetService(viewModelRequest.ViewModelType);

                return viewModel;
            });
        }
    }
}
