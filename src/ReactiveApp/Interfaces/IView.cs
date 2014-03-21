using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Interfaces
{
    public interface IView
    {
        IShell Shell { get; set; }
    }
}
