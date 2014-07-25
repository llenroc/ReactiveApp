using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Munq;
using Splat;

namespace ReactiveApp
{
    public class MunqDependencyResolver : IMutableDependencyResolver
    {
        private readonly IocContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="MunqDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public MunqDependencyResolver(IocContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
            this.container.DefaultLifetimeManager = new AlwaysNewLifetime();
        }

        public void Register(Func<object> factory, Type serviceType, string contract = null)
        {
            container.Register(contract, serviceType, m => factory());
        }

        public object GetService(Type serviceType, string contract = null)
        {
            try
            {
                return container.Resolve(contract, serviceType);
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public IEnumerable<object> GetServices(Type serviceType, string contract = null)
        {
            try
            {
                return container.ResolveAll(contract, serviceType);
            }
            catch (KeyNotFoundException)
            {
                return new object[0];
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.container.Dispose();
        }

        public IDisposable ServiceRegistrationCallback(Type serviceType, string contract, Action<IDisposable> callback)
        {
            throw new NotImplementedException();
        }
    }
}
