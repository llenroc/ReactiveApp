using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveApp.ViewModels
{
    /// <summary>
    /// ActivationInfo sent during activation.
    /// </summary>
    public class ActivationInfo
   {
        /// <summary>
        /// Indicates whether the sender was initialized in addition to being activated.
        /// </summary>
        public bool WasInitialized;
    }
}
