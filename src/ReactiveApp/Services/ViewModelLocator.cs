using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public ViewModelLocator(ISerializer serializer)
        {
            this.ContractKey = "ReactiveApp_ViewModelContract";
        }

        public string ContractKey { get; set; }

        public object GetViewModelForViewModelType(Type viewModelType, IDataContainer parameters)
        {
            object viewModel;

            string contract;
            if (parameters != null && parameters.Data.TryGetValue(ContractKey, out contract))
            {
                viewModel = Locator.Current.GetService(viewModelType, contract);
            }
            else
            {
                viewModel = Locator.Current.GetService(viewModelType);
            }

            if (parameters != null)
            {
                this.InitializePropertiesFromParameters(viewModel, parameters);
            }

            return viewModel;
        }

        protected virtual void InitializePropertiesFromParameters(object viewModel, IDataContainer parameters)
        {
            Type viewModelType = viewModel.GetType();

            foreach (var kvp in parameters.Data)
            {
                PropertyInfo property = viewModelType.GetRuntimeProperty(kvp.Key);
                if(property != null)
                {
                    if(property.PropertyType == typeof(string))
                    {
                        property.SetValue(viewModel, kvp.Value);
                    }
                    else
                    {
                        
                    }
                }
            }
        }
    }
}
