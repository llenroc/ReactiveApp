using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Xaml.Services
{
    public class PhoneReactiveViewModelRequestTranslator : IPhoneReactiveViewModelRequestTranslator
    {
        private readonly IViewLocator viewLocator;
        private readonly INavigationSerializer navigationSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneReactiveViewModelRequestTranslator"/> class.
        /// </summary>
        /// <param name="viewLocator">The view locator.</param>
        public PhoneReactiveViewModelRequestTranslator(IViewLocator viewLocator, INavigationSerializer navigationSerializer)
        {
            this.viewLocator = viewLocator;
            this.navigationSerializer = navigationSerializer;
        }

        public virtual Uri GetUriForViewModelRequest(ReactiveViewModelRequest request)
        {
            var viewType = viewLocator.GetViewTypeForViewModelType(request.ViewModelType);
            if(viewType == null)
            {
                throw new InvalidOperationException("No view type found for" + request.ViewModelType);
            }

            string requestString = this.navigationSerializer.SerializeObject(request);
            string uriString = string.Format("{0}?request={1}", this.GetUriPartForView(viewType), Uri.EscapeDataString(requestString));
            return new Uri(uriString, UriKind.Relative);
        }

        protected virtual string GetUriPartForView(Type viewType)
        {
            var segments = viewType.FullName.Split('.');
            var folderAndNamespace = segments.SkipWhile((segment) => segment != ViewsFolderName);
            var viewUriPart = string.Format("/{0}.xaml", string.Join("/", folderAndNamespace));
            return viewUriPart;
        }

        public ReactiveViewModelRequest GetViewModelRequestForUri(Uri requestUri)
        {
            var queryParams = requestUri.ParseQueryString();

            string queryString = null;
            if (!queryParams.TryGetValue("request", out queryString))
            {
                throw new ArgumentException("requestUri doe not contain a ReactiveViewModelRequest");
            }

            var text = Uri.UnescapeDataString(queryString);
            return navigationSerializer.DeserializeObject<ReactiveViewModelRequest>(text);
        }

        protected virtual string ViewsFolderName
        {
            get { return "Views"; }
        }
    }
}
