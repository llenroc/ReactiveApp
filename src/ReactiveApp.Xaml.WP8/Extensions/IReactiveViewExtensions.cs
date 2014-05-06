using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using ReactiveApp.Xaml.Services;
using Splat;

namespace ReactiveApp.Xaml
{
    public static class IReactiveViewExtensions
    {
        public static ReactiveViewModelRequest ViewCreated(this IReactiveView This, Uri viewModelRequestUri)
        {
            // on Windows Phone Silveright we need to parse the Uri into a ReactiveViewModelRequest
            var translatorService = Locator.Current.GetService<IPhoneViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetViewModelRequestForUri(viewModelRequestUri);

            This.ViewCreated(viewModelRequest);

            return viewModelRequest;           
        }
    }
}
