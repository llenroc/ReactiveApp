using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Services
{
    public interface IViewModelLocator
    {
        object GetViewModelForViewModelType(Type viewModel, IDataContainer parameters);
    }
}
