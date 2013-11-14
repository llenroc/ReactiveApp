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
        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <param name="callback">The implementer calls this action with the result of the close check.</param>
         bool CanClose();
    }
}
