using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using Splat;

namespace ReactiveApp.Services
{
    public class ViewModelLocator : IViewModelLocator
    {
        public object GetViewModelForViewModelType(Type viewModel, IDataContainer parameters)
        {
            return Locator.Current.GetService(viewModel);
        }
    }
}
