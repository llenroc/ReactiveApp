using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.ViewModels
{
    public class ReactiveViewModelRequest
    {
        public ReactiveViewModelRequest(Type viewModelType, IDataContainer parameters,
            IDataContainer presentationInfo)
        {
            ViewModelType = viewModelType;
            Parameters = parameters;
            PresentationInfo = presentationInfo;
        }

        public Type ViewModelType { get; set; }
        public IDataContainer Parameters { get; set; }
        public IDataContainer PresentationInfo { get; set; }
    }

    public class ReactiveViewModelRequest<TViewModel> : ReactiveViewModelRequest where TViewModel : IReactiveViewModel
    {
        public ReactiveViewModelRequest(IDataContainer parameters, IDataContainer viewModelState)
            : base(typeof(TViewModel), parameters, viewModelState)
        {
        }
    }
}
