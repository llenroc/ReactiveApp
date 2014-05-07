using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using ReactiveApp.Android.Services;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using Splat;

namespace ReactiveApp.Android
{
    public static class IReactiveViewExtensions
    {
        public static ReactiveViewModelRequest ViewCreated(this IReactiveView This, Intent intent)
        {
            // on Android we need to parse the Intent into a ReactiveViewModelRequest
            var translatorService = Locator.Current.GetService<IAndroidReactiveViewModelRequestTranslator>();
            var viewModelRequest = translatorService.GetViewModelRequestForIntent(intent);

            This.ViewCreated(viewModelRequest);

            return viewModelRequest;
        }
    }
}
