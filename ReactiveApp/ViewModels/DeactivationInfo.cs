using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.ViewModels
{
    /// <summary>
    /// DeactivationInfo sent during deactivation.
    /// </summary>
    public class DeactivationInfo
    {
        /// <summary>
        /// Indicates whether the sender was closed in addition to being deactivated.
        /// </summary>
        public bool WasClosed;
    }
}
