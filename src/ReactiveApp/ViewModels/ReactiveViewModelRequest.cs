using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.ViewModels
{
    public class ReactiveViewModelRequest
    {
        public ReactiveViewModelRequest(Type viewModelType, IDictionary<string, string> parameters,
            IDictionary<string, string> viewModelState)
        {
            ViewModelType = viewModelType;
            Parameters = parameters;
            ViewModelState = viewModelState;
        }

        public Type ViewModelType { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
        public IDictionary<string, string> ViewModelState { get; set; }
    }

    public class ReactiveViewModelRequest<TViewModel> : ReactiveViewModelRequest where TViewModel : IReactiveViewModel
    {
        public ReactiveViewModelRequest(IDictionary<string, string> parameters, IDictionary<string, string> viewModelState)
            : base(typeof(TViewModel), parameters, viewModelState)
        {
        }
    }
}
