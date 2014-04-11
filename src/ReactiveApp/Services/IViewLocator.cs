using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Services
{
    public interface IViewLocator
    {
        Type GetViewTypeForViewModel(Type viewModel = null);
    }
}
