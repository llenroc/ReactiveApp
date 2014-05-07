using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;
using Splat;

namespace ReactiveApp.Android.Services
{
    public class AndroidReactiveViewModelRequestTranslator : IAndroidReactiveViewModelRequestTranslator
    {
        private readonly Application application;
        private readonly IViewLocator viewLocator;
        private readonly INavigationSerializer navigationSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndroidReactiveViewModelRequestTranslator"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="viewLocator">The view locator.</param>
        /// <param name="navigationSerializer">The navigation serializer.</param>
        public AndroidReactiveViewModelRequestTranslator(Application application, IViewLocator viewLocator, INavigationSerializer navigationSerializer)
        {
            this.application = application;
            this.viewLocator = viewLocator;
            this.navigationSerializer = navigationSerializer;
        }

        public Intent GetIntentForViewModelRequest(ReactiveViewModelRequest viewModelRequest)
        {
            var viewType = this.viewLocator.GetViewTypeForViewModel(viewModelRequest.ViewModelType);
            if (viewType == null)
            {
                throw new InvalidOperationException("No view type found for" + viewModelRequest.ViewModelType);
            }

            string requestString = this.navigationSerializer.SerializeObject(viewModelRequest);
            Intent intent = new Intent(this.application.ApplicationContext, viewType);
            intent.PutExtra("request", requestString);

            return intent;
        }

        public ReactiveViewModelRequest GetViewModelRequestForIntent(Intent intent)
        {
            if(intent.Extras == null)
            {
                return null;
            }
            var extraData = intent.Extras.GetString("request");
            if (extraData == null)
            {
                return null;
            }

            var serializer = Locator.Current.GetService<INavigationSerializer>();
            var viewModelRequest = serializer.DeserializeObject<ReactiveViewModelRequest>(extraData);
                        
            return viewModelRequest;
        }
    }
}