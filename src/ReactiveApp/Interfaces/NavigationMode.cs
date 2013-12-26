using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Interfaces
{
    /// <summary>
    /// Specifies the type of navigation that is occurring.
    /// </summary>
    public enum NavigationMode
    { 
        New = 0,
        
        Back = 1,
 
        Forward = 2
    }
}
