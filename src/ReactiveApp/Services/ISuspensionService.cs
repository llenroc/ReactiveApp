using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Services
{
    /// <summary>
    /// Nicked from http://caliburnmicro.codeplex.com/wikipage?title=Working%20with%20Windows%20Phone%207%20v1.1
    ///
    /// Launching - Occurs when a fresh instance of the application is launching.
    /// Activated - Occurs when a previously paused/tombstoned app is resumed/resurrected.
    /// Deactivated - Occurs when the application is being paused or tombstoned.
    /// Closing - Occurs when the application is closing.
    /// Continuing - Occurs when the app is continuing from a temporarily paused state.
    /// Continued - Occurs after the app has continued from a temporarily paused state.
    /// Resurrecting - Occurs when the app is "resurrecting" from a tombstoned state.
    /// Resurrected - Occurs after the app has "resurrected" from a tombstoned state.
    ///
    /// </summary>
    public interface ISuspensionService
    {
        /// <summary>
        /// Occurs when a fresh instance of the application is launching.
        /// </summary>
        IObservable<string> IsLaunchingNew { get; }


        /// <summary>
        /// Occurs when a previously paused/tombstoned app is resumed/resurrected.
        /// </summary>
        IObservable<string> IsResuming { get; }

        /// <summary>
        /// Occurs when an app is resumed without being closed or stopped.
        /// </summary>
        IObservable<string> IsUnpausing { get; }

        /// <summary>
        /// Occurs when an app is deactivated or closed an state should be saved.
        /// </summary>
        IObservable<IDisposable> ShouldPersistState { get; }

        /// <summary>
        /// Occurs when the app gets into an unknown state due to unhandled exceptions.
        /// </summary>
        IObservable<Unit> ShouldInvalidateState { get; }
    }
}
