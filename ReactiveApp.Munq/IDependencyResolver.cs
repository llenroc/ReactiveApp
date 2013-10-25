// --------------------------------------------------------------------------------------------------
// © Copyright 2011 by Matthew Dennis.
// Released under the Microsoft Public License (Ms-PL) http://www.opensource.org/licenses/ms-pl.html
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Munq
{
	/// <summary>
    ///     This interface defines the IOC container's functionality to resolve instances by type
	/// 	and distinguishing name.
	/// </summary>>
    public interface IDependencyResolver
	{
		//Resolve

		/// <summary>Resolves the unnamed instance of the type TType.</summary>
		/// <typeparam name="TType">The type that is to be resolved.</typeparam>
		/// <returns>An instance of the class registered to resolve the type TType.</returns>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered 
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DefaultLogger&lt;>();
		/// 		...
		/// 		ILogger logger = container.Resolve&lt;ILogger&lt;>();
		/// 		logger.log(Log.Info, "Logging is active.");
		/// 	</code>
		/// </example>
		TType Resolve<TType>() where TType : class;
		
        /// <summary>Resolves the named instance of the type TType.</summary>
		/// <typeparam name="TType">The type that is to be resolved.</typeparam>
		/// <param name="name">The name of the registration for type TType.</param>
		/// <returns>An instance of the class registered to resolve the type TType.</returns>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DatabaseLogger&lt;>("Database");
		/// 		...
		/// 		ILogger logger = container.Resolve&lt;ILogger&lt;>("Database");
		/// 		logger.log(Log.Info, "Logging is active.");
		/// 	</code>
		/// </example>
		TType Resolve<TType>(string name) where TType : class;
		
        /// <summary>Resolves the unnamed instance of the specified type.</summary>
		/// <param name="type">The type that is to be resolved.</param>
		/// <returns>An instance of the class registered to resolve the type.</returns>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DefaultLogger&lt;>();
		/// 		...
		/// 		ILogger logger = container.Resolve(typeof(ILogger));
		/// 		logger.log(Log.Info, "Logging is active.");
		/// 	</code>
		/// </example>
		object Resolve(Type type);
		
        /// <summary>Resolves the named instance of the specified type.</summary>
		/// <param name="type">The type that is to be resolved.</param>
		/// <param name="name">The name of the registration for type.</param>
		/// <returns>An instance of the class registered to resolve the type TType.</returns>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DatabaseLogger&lt;>("Database");
		/// 		...
		/// 		ILogger logger = container.Resolve("Database", typeof(ILogger));
		/// 		logger.log(Log.Info, "Logging is active.");
		/// 	</code>
		/// </example>
		object Resolve(string name, Type type);

		//ResolveAll
		
        /// <summary>Gets of all possible named and unnamed resolutions for the type TType.</summary>
		/// <typeparam name="TType">The type of which to get the instances.</typeparam>
		/// <returns>A list of resolved instances.</returns>
		/// <example>
		/// 	This example gets a list of plugins and initializes them.
		/// 	<code>
		/// 		...
		/// 		var plugins = container.ResolveAll&lt;IPlugin&gt;();
		/// 		foreach (var plugin in plugins)
		/// 			MyApp.Initialize(plugin);
		/// 	</code>
		/// </example>
		IEnumerable<TType> ResolveAll<TType>() where TType : class;
		
        /// <summary>Gets of all possible named and unnamed resolutions for the specified type.</summary>
		/// <param name="type">The type of which to get the instances.</param>
		/// <returns>A list of resolved instances.</returns>
		/// <example>
		/// 	This example gets a list of plugins and initializes them.
		/// 	<code>
		/// 		...
		/// 		var plugins = container.ResolveAll(typeof(IPlugin));
		/// 		foreach (var plugin in plugins)
		/// 		MyApp.Initialize(plugin);
		/// 	</code>
		/// </example>
		IEnumerable<object> ResolveAll(Type type);

		//Lazy Resolve
		
        /// <summary>
		/// 	Gets a function that resolves the unnamed instance of the type TType.
		/// 	Used for delaying creating expensive resources until if and when required.	
		/// </summary>
		/// <typeparam name="TType">The type that is to be resolved.</typeparam>
		/// <returns>An instance of the class registered to resolve the type TType.</returns>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DefaultLogger&lt;>();
		/// 		...
		/// 		Func&lt;ILogger&gt; loggerFunc = container.LazyResolve&lt;ILogger&lt;>();
		/// 		...
		/// 		var logger = loggerFunc();
		/// 		logger.log(Log.Info, "Logging is active.");
		/// 	</code>
		/// </example>
		Func<TType> LazyResolve<TType>() where TType : class;
		
        /// <summary>
	    ///     Gets a function that resolves the named instance of the type TType.
		/// 	Used for delaying creating expensive resources until if and when required.
		/// </summary>
		/// <typeparam name="TType">The type that is to be resolved.</typeparam>
		/// <param name="name">The name of the registration for type TType.</param>
		/// <returns>An instance of the class registered to resolve the type TType.</returns>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DatabaseLogger&lt;>("Database");
		/// 		...
		/// 		Func&lt;ILogger&gt; loggerFunc = container.LazyResolve&lt;ILogger&lt;>("Database");
		/// 		...
		/// 		var logger = loggerFunc();
		/// 		logger.log(Log.Info, "Database Logging is active.");
		/// 	</code>
		/// </example>
		Func<TType> LazyResolve<TType>(string name) where TType : class;
		
        /// <summary>
		/// 	Gets a function that resolves the unnamed instance of the specified type.
		/// 	Used for delaying creating expensive resources until if and when required.
		/// </summary>
		/// <param name="type">The type that is to be resolved.</param>
		/// <returns>An instance of the class registered to resolve the specified type.</returns>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DefaultLogger&lt;>();
		/// 		...
		/// 		Func&lt;ILogger&gt; loggerFunc = container.LazyResolve&lt;ILogger&lt;>();
		/// 		...
		/// 		var logger = loggerFunc();
		/// 		logger.log(Log.Info, "Logging is active.");
		/// 	</code>
		/// </example>
		Func<object> LazyResolve(Type type);
		
        /// <summary>
		/// 	Gets a function that resolves the named instance of the specified type.
		/// 	Used for delaying creating expensive resources until if and when required.
		/// </summary>
		/// <param name="name">The name of the registration for specified type.</param>
		/// <param name="type">The type that is to be resolved.</param>
		/// <returns>An instance of the class registered to resolve the specified type.</returns>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DatabaseLogger&lt;>("Database");
		/// 		...
		/// 		Func&lt;ILogger&gt; loggerFunc = container.LazyResolve("Database", typeof(ILogger));
		/// 		... 
		/// 		var logger = loggerFunc();
		/// 		logger.log(Log.Info, "Database Logging is active.");
		/// 	</code>
		/// </example>
		Func<object> LazyResolve(string name, Type type);

		//CanResolve

		/// <summary>Determines if the type TType can be resolved.</summary>
		/// <typeparam name="TType">The type that is to be resolved.</typeparam>
		/// <returns>True if there is an unnamed Registration for the type TType, false otherwise.</returns>
		/// <remarks>If TType is a class, it may still be possible to resolve it.</remarks>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DefaultLogger&lt;>();
		/// 		...
		/// 		if (!container.CanResolve&lt;ILogger&gt;())
		/// 			throw new Exception("Logging is not available");
		/// 	</code>
		/// </example>
		bool CanResolve<TType>() where TType : class;
		
        /// <summary>Determines if the type TType can be resolved.</summary>
		/// <typeparam name="TType">The type that is to be resolved.</typeparam>
		/// <param name="name">The name of the Registration to check for.</param>
		/// <returns>True if there is an named Registration for the type TType, false otherwise.</returns>
		/// <remarks>If TType is a class, it may still be possible to resolve it.</remarks>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DatabaseLogger&lt;>("Database");
		/// 		...
		/// 		if (!container.CanResolve&lt;ILogger&gt;("Database"))
		/// 			throw new Exception("Database Logging is not available");
		/// 	</code>
		/// </example>
		bool CanResolve<TType>(string name) where TType : class;
		
        /// <summary>Determines if the specified type can be resolved.</summary>
		/// <param name="type">The type that is to be resolved.</param>
		/// <returns>True if there is an unnamed Registration for the specified type, false otherwise.</returns>
		/// <remarks>If specified type is a class, it may still be possible to resolve it.</remarks>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DefaultLogger&lt;>();
		/// 		...
		/// 		if (!container.CanResolve(typeof(ILogger))
		/// 			throw new Exception("Logging is not available");
		/// 	</code>
		/// </example>
		bool CanResolve(Type type);
		
        /// <summary>Determines if the specified type can be resolved.</summary>
		/// <param name="name">The name of the Registration to check for.</param>
		/// <param name="type">The type that is to be resolved.</param>
		/// <returns>True if there is an named Registration for the specified type, false otherwise.</returns>
		/// <remarks>If the specified type is a class, it may still be possible to resolve it.</remarks>
		/// <example>
		/// 	This example gets an instance of the class registered , or result of the function registered
		/// 	for the type.
		/// 	<code>
		/// 		...
		/// 		container.Register&lt;ILogger, DatabaseLogger&lt;>("Database");
		/// 		...
		/// 		if (!container.CanResolve&lt;ILogger&gt;("Database"))
		/// 			throw new Exception("Database Logging is not available");
		/// 	</code>
		/// </example>
		bool CanResolve(string name, Type type);
	}
}
