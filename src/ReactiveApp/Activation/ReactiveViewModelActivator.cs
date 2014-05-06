using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using ReactiveUI;

namespace ReactiveApp.Activation
{
    public class ReactiveViewModelActivator : IViewModelActivator
    {
        private readonly List<Func<IDataContainer, IDataContainer, IEnumerable<IDisposable>>> activationBlocks;
        private readonly ISupportsActivation activation;
        private IDisposable activationHandle = Disposable.Empty;
        private int refCount = 0;

        public ReactiveViewModelActivator(ISupportsActivation activation)
        {
            this.activationBlocks = new List<Func<IDataContainer, IDataContainer, IEnumerable<IDisposable>>>();
            this.activation = activation;
        }

        public void AddActivationBlock(Func<IDataContainer, IDataContainer, IEnumerable<IDisposable>> block)
        {
            this.activationBlocks.Add(block);
        }

        public virtual IDisposable Activate(IDataContainer parameters, IDataContainer state)
        {
            if (Interlocked.Increment(ref this.refCount) == 1)
            {
                var disp = new CompositeDisposable(this.activationBlocks.SelectMany(x => x(parameters, state)).Concat(new IDisposable[] { this.activation.Activator.Activate() }));
                Interlocked.Exchange(ref this.activationHandle, disp).Dispose();
            }

            return Disposable.Create(() => this.Deactivate());
        }

        public void Deactivate(bool ignoreRefCount = false)
        {
            if (Interlocked.Decrement(ref refCount) == 0 || ignoreRefCount)
            {
                Interlocked.Exchange(ref this.activationHandle, Disposable.Empty).Dispose();
            }
        }
    }
}
