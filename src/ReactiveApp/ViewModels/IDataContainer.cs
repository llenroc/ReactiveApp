using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.ViewModels
{
    public interface IDataContainer
    {
        IDictionary<string, string> Data { get; }
    }
}
