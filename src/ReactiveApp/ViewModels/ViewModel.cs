using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Platform;
using ReactiveUI;
using Splat;

namespace ReactiveApp.ViewModels
{
    public class ViewModel : ReactiveObject, IActivate, IDeactivate, IClose, IViewAware, IEnableLogger
    {
        private object view;
        private ISubject<ActivationInfo> activated;
        private ISubject<DeactivationInfo> deactivated;
        private ISubject<ViewAttachedInfo> viewAttached;
        private ISubject<object> viewLoaded;
        private ISubject<object> viewReady;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        /// <param name="cacheView">if set to <c>true</c> [cache view].</param>
        public ViewModel(bool cacheView = true)
        {
            this.activated = new Subject<ActivationInfo>();
            this.deactivated = new Subject<DeactivationInfo>();
            this.viewAttached = new Subject<ViewAttachedInfo>();
            this.viewLoaded = new Subject<object>();
            this.viewReady = new Subject<object>();
            this.CacheView = cacheView;

            // make view null 
            this.WhenAnyValue(x => x.CacheView).Subscribe(doCache =>
            {
                if (!doCache)
                {
                    view = null;
                }
            });

            this.Log().Info("Created {0}, cache view: {1}", this, cacheView);
        }

        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected virtual void Initialize() { }

        private bool isInitialized;
        /// <summary>
        /// Indicates whether or not this instance is currently initialized.
        /// </summary>
        public bool IsInitialized
        {
            get { return isInitialized; }
            private set { this.RaiseAndSetIfChanged(ref isInitialized, value); }
        }

        private bool isActive;
        /// <summary>
        /// Indicates whether or not this instance is currently active.
        /// </summary>
        public bool IsActive
        {
            get { return isActive; }
            private set { this.RaiseAndSetIfChanged(ref isActive, value); }
        }
        
        private bool cacheView;
        ///<summary>
        ///  Indicates whether or not this instance maintains a view cache.
        ///</summary>
        protected bool CacheView
        {
            get { return cacheView; }
            set { this.RaiseAndSetIfChanged(ref cacheView, value); }
        }

        #region Interfaces

        #region IActivate
        
        public void Activate()
        {
            // we cant activate twice
            if (IsActive)
            {
                return;
            }

            //initialize once per instance
            var initialized = false;
            if (!IsInitialized)
            {
                IsInitialized = initialized = true;
                Initialize();
            }

            IsActive = true;
            this.Log().Info("Activating {0}.", this);

            this.activated.OnNext(new ActivationInfo() { WasInitialized = initialized });
        }

        public IObservable<ActivationInfo> Activated
        {
            get { return this.activated; }
        }

        #endregion

        #region IDeactivate

        public void Deactivate(bool close)
        {
            if (IsActive || (IsInitialized && close))
            {
                IsActive = false;
                this.Log().Info("Deactivating {0}.", this);
                
                this.deactivated.OnNext(new DeactivationInfo() { WasClosed = close });

                if (close)
                {
                    view = null;
                    this.Log().Info("Closed {0}.", this);
                }
            }
        }

        public IObservable<DeactivationInfo> Deactivated
        {
            get { return this.deactivated; }
        }

        #endregion

        #region IClose

        public virtual bool CanClose()
        {
            return true;
        }

        #endregion

        #region IViewAware

        public void AttachView(object view)
        {
            if (this.CacheView)
            {
                this.view = view;
            }

            PlatformProvider.Instance.ViewEvents.OnFirstLoaded(view).Multicast(this.viewLoaded).PermaRef();
            this.viewAttached.OnNext(new ViewAttachedInfo() { View = view });

            // if we are not active yet, we wait for the Activated message.
            if (this.IsActive)
            {
                PlatformProvider.Instance.ViewEvents.OnLayoutUpdated(view).Multicast(this.viewReady).PermaRef();
            }
            else
            {
                // we don't want to keep a ref to the view here
                WeakReference viewRef = new WeakReference(view);
                this.Activated.FirstOrDefaultAsync().SelectMany(_ =>
                {
                    if (viewRef.Target != null)
                    {
                        return PlatformProvider.Instance.ViewEvents.OnLayoutUpdated(view).Multicast(this.viewReady).PermaRef();
                    }
                    else
                    {
                        return Observable.Empty<object>();
                    }
                });
            }
        }

        public object GetView()
        {
            return view;
        }

        public IObservable<ViewAttachedInfo> ViewAttached
        {
            get { return this.viewAttached; }
        }

        protected IObservable<object> ViewLoaded
        {
            get { return this.viewLoaded; }
        }

        protected IObservable<object> ViewReady
        {
            get { return this.viewReady; }
        }

        #endregion

        #endregion
    }
}
