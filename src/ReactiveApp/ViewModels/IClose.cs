using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveApp.ViewModels
{
    /// <summary>
    /// Denotes an instance which may prevent closing.
    /// </summary>
    public interface IClose
    {
         bool CanClose();
    }
}
