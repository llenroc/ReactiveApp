using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
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
                var viewModelLocator = Locator.Current.GetService<IViewModelLocator>();
                var viewModel = viewModelLocator.GetViewModelForViewModelType(viewModelRequest.ViewModelType, viewModelRequest.Parameters);

                return viewModel;
            });
        }
    }
}
