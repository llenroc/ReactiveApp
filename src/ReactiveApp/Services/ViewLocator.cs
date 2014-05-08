using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;

namespace ReactiveApp.Services
{
    public class ViewLocator : IViewLocator
    {
        public virtual Type GetViewTypeForViewModelType(Type viewModel, string contract = null)
        {
            // Given IFooBarViewModel (whose name we derive from T), we'll look 
            // for a few things:
            // * IFooBarView that implements IViewFor
            // * IViewFor<IFooBarViewModel>
            // * IViewFor<FooBarViewModel> (the original behavior in RxUI 3.1)

            contract = contract ?? viewModel.GetTypeInfo().GetCustomAttributes<ViewContractAttribute>(true).Select(attr => attr.Contract).FirstOrDefault();

            Type viewType = typeof(IViewFor<>);
            Type genericViewType = viewType.MakeGenericType(viewModel);

            return Locator.Current.GetService(genericViewType, contract) as Type;
        }
    }
}
