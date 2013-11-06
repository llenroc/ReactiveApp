using System.ComponentModel;

namespace ReactiveApp.Platform
{
    /// <summary>
    /// Provides an interface to use Platform specific services in a portable class library.
    /// </summary>
    /// <remarks>
    /// This type is used by the ReactiveApp infrastructure and not meant for public consumption or implementation.
    /// No guarantees are made about forward compatibility of the type's functionality and its usage.
    /// </remarks>
    public interface IPlatformProvider
    {
        /// <summary>
        /// Returns a platform-specific implemention to attach to view events.
        /// </summary>
        IViewEvents ViewEvents { get; }
    }
}
