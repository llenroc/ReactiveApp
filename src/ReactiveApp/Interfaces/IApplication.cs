using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Interfaces
{
    public interface IApplication<T, U>        
        where T : class, IShell<T, U>
        where U : class, IView<T, U>
    {
        /// <summary>
        /// Gets the shell.
        /// </summary>
        /// <value>
        /// The shell.
        /// </value>
        T Shell { get; }
    }
}
