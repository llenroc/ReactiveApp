using System;
using System.Runtime.CompilerServices;
using Munq.LifetimeManagers;

namespace Munq
{
	public static class LifetimeExtensions
	{		
		readonly static ContainerLifetime containerLifetime   = new ContainerLifetime();

		public static IRegistration AsAlwaysNew(this IRegistration reg)
		{
			return reg.WithLifetimeManager(null);
		}

		public static IRegistration AsContainerSingleton(this IRegistration reg)
		{
			return reg.WithLifetimeManager(containerLifetime);
		}
	}
}
