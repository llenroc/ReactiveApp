using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReactiveApp.ViewModels
{
    /// <summary>
    /// Denotes an instance which requires deactivation.
    /// </summary>
    public interface IDeactivate
    {
        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        /// <param name="close">Indicates whether or not this instance is being closed.</param>
        void Deactivate(bool close);

        /// <summary>
        /// Raised after deactivation.
        /// </summary>
        IObservable<DeactivationInfo> Deactivated;
    }
}
