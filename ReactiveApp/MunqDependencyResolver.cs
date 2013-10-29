using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp
{
    class MunqDependencyResolver : IMutableDependencyResolver
    {
        private readonly Munq.IocContainer container;

        public MunqDependencyResolver(Munq.IocContainer container)
        {
            Contract.Requires(container != null);

            this.container = container;
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

        public void Dispose()
        {
            this.container.Dispose();
        }
    }
}
