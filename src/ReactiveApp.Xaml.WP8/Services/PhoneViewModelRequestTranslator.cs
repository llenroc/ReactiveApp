using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Xaml.Services
{
    public class PhoneViewModelRequestTranslator : IPhoneViewModelRequestTranslator
    {
        private readonly IViewLocator viewLocator;
        private readonly INavigationSerializer navigationSerializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneViewModelRequestTranslator"/> class.
        /// </summary>
        /// <param name="viewLocator">The view locator.</param>
        public PhoneViewModelRequestTranslator(IViewLocator viewLocator, INavigationSerializer navigationSerializer)
        {
            this.viewLocator = viewLocator;
            this.navigationSerializer = navigationSerializer;
        }

        public virtual Uri GetUriForViewModelRequest(ReactiveViewModelRequest request)
        {
            var viewType = viewLocator.GetViewTypeForViewModel(request.ViewModelType);
            if(viewType == null)
            {
                throw new InvalidOperationException("No view type found for" + request.ViewModelType);
            }

            string requestString = this.navigationSerializer.SerializeObject(request);
            string uriString = string.Format("/{0}?request={1}", this.GetUriPartForView(viewType), requestString);
            return new Uri(uriString, UriKind.Relative);
        }

        protected virtual string GetUriPartForView(Type viewType)
        {
            var segments = viewType.FullName.Split('.');
            var folderAndNamespace = segments.SkipWhile((segment) => segment != ViewsFolderName);
            var viewUriPart = string.Format("/{0}.xaml", string.Join("/", folderAndNamespace));
            return viewUriPart;
        }

        protected virtual string ViewsFolderName
        {
            get { return "Views"; }
        }
    }
}
