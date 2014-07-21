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
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelLocator"/> class.
        /// </summary>
        public ViewModelLocator()
        {
            this.ContractKey = "ReactiveApp_ViewModelContract";
        }

        public string ContractKey { get; set; }

        public object GetViewModelForViewModelType(Type viewModel, IDataContainer parameters)
        {
            string contract;
            if (parameters.Data.TryGetValue(ContractKey, out contract))
            {
                return Locator.Current.GetService(viewModel, contract);
            }
            else
            {
                return Locator.Current.GetService(viewModel);
            }
        }
    }
}
