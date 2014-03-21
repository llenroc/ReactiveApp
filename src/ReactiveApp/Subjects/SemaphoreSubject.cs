using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;

namespace ReactiveApp.Subjects
{
    internal class SemaphoreSubject<T> : ISubject<T>, IEnableLogger
    {
        private readonly ISubject<T> inner;
        private readonly int maxCount;
        private Queue<T> nextItems = new Queue<T>();
        private int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="SemaphoreSubject{T}"/> class.
        /// </summary>
        /// <param name="maxCount">The maximum count.</param>
        /// <param name="sched">The sched.</param>
        public SemaphoreSubject(int maxCount, IScheduler sched = null)
        {
            this.Log().Debug("maxCount is '{0}'", maxCount);
            this.inner = (sched != null ? (ISubject<T>)new ScheduledSubject<T>(sched) : new Subject<T>());
            this.maxCount = maxCount;
        }

        public void OnNext(T value)
        {
            var queue = Interlocked.CompareExchange(ref nextItems, null, null);
            if (queue == null)
            {
                return;
            }

            lock (queue)
            {
                this.Log().Debug("OnNext called for '{0}', count is '{1}'", value, count);
                queue.Enqueue(value);
            }
            yieldUntilEmptyOrBlocked();
        }

        public void Release()
        {
            Interlocked.Decrement(ref count);

            this.Log().Debug("Releasing, count is now {0}", count);
            yieldUntilEmptyOrBlocked();
        }

        public void OnCompleted()
        {
            var queue = Interlocked.Exchange(ref nextItems, null);
            if (queue == null)
            {
                return;
            }

            T[] items;
            lock (queue)
            {
                items = queue.ToArray();
            }

            foreach (var v in items)
            {
                this.inner.OnNext(v);
            }

            this.inner.OnCompleted();
        }

        public void OnError(Exception error)
        {
            var queue = Interlocked.Exchange(ref nextItems, null);
            this.inner.OnError(error);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            return this.inner.Subscribe(observer);
        }

        void yieldUntilEmptyOrBlocked()
        {
            var queue = Interlocked.CompareExchange(ref nextItems, null, null);

            if (queue == null)
            {
                return;
            }

            while (count < maxCount)
            {
                T next;
                lock (queue)
                {
                    if (queue.Count == 0)
                    {
                        break;
                    }
                    next = queue.Dequeue();
                }

                this.Log().Debug("Yielding '{0}', _count = {1}, _maxCount = {2}", next, count, maxCount);
                this.inner.OnNext(next);

                if (Interlocked.Increment(ref count) >= maxCount)
                {
                    break;
                }
            }
        }
    }
}
