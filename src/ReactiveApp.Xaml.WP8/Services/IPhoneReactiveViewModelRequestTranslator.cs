using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Xaml.Services
{
    public interface IPhoneReactiveViewModelRequestTranslator
    {
        Uri GetUriForViewModelRequest(ReactiveViewModelRequest request);

        ReactiveViewModelRequest GetViewModelRequestForUri(Uri requestUri);
    }
}
