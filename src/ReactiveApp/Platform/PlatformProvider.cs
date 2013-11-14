using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ReactiveApp.Platform
{
    /// <summary>
    /// Provides access to platform-specific framework API's.
    /// </summary>
    /// <remarks>
    /// This type is used by the ReactiveApp infrastructure and not meant for public consumption or implementation.
    /// </remarks>
    public static class PlatformProvider
    {
        private static IPlatformProvider current;

        /// <summary>
        /// Name of the assembly containing the Class with the <see cref="PlatformProvider.PlatformTypeFullName"/> name.
        /// </summary>
        public static string PlatformAssemblyName = "ReactiveApp.Xaml";

        /// <summary>
        /// Name of the type implementing <see cref="IPlatformProvider"/>.
        /// </summary>
        public static string PlatformTypeFullName = "ReactiveApp.Xaml.CurrentPlatformProvider";

        /// <summary>
        /// Gets the current platform. If none is loaded yet, accessing this property triggers platform resolution.
        /// </summary>
        public static IPlatformProvider Instance
        {
            get
            {
                // create if not yet created
                if (current == null)
                {
                    //assume the platform assembly has the same key, same version and same culture
                    // as the assembly where the IPlatform interface lives.
                    var provider = typeof(IPlatformProvider);
                    var asm = new AssemblyName(provider.GetTypeInfo().Assembly.FullName);
                    //change name to the specified name
                    asm.Name = PlatformAssemblyName;
                    var name = PlatformTypeFullName + ", " + asm.FullName;

                    //look for the type information but do not throw if not found
                    var type = Type.GetType(name, false);
                    if (type != null)
                    {
                        // create type
                        // since we are the only one implementing this interface
                        // this cast is safe.
                        current = (IPlatformProvider)Activator.CreateInstance(type);
                    }
                    else
                    {
                        // throw
                        ThrowForMissingPlatformAssembly();
                    }
                }

                return current;
            }

            // keep this public so we can set a Platform for unit testing.
            set
            {
                current = value;
            }
        }

        /// <summary>
        /// Method to throw an exception in case no Platform assembly could be found.
        /// </summary>
        private static void ThrowForMissingPlatformAssembly()
        {
            throw new InvalidOperationException("Can't find the CurrentPlatform class for the current platform we are running on.");
        }
    }
}
