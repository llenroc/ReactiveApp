using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Interfaces
{
    public interface IView<T, U>
        where T : class, IShell<T, U>
        where U : class, IView<T, U>
    {
        T Shell { get; set; }
    }
}
