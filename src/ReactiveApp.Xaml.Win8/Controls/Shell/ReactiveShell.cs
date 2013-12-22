using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

using ReactiveApp.Interfaces;
using ReactiveUI;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
#else
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using ReactiveApp.Subjects;
#endif

namespace ReactiveApp.Xaml.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ReactiveShell : ContentControl, IShell<ReactiveShell, ReactiveView>, IEnableLogger
    {
        #region Fields

        private static readonly CacheMode BitmapCacheMode = new BitmapCache();
        private const string ViewPresentersPanelName = "PART_ViewPresentersPanel";
        private ContentPresenter currentViewPresenter;
        private Panel viewPresentersPanel;
        private IList<Action<Panel>> overlays;

        private readonly SemaphoreSubject<Tuple<IJournalEntry, NavigationMode>> navigations;
        private readonly IObservable<Tuple<IJournalEntry, ReactiveView, bool>> journal;
        private readonly ISubject<bool> isNavigationActive;

        private readonly Lazy<Subject<NavigatingInfo>> navigating;
        private readonly Lazy<Subject<NavigatedInfo>> navigated;
        private readonly ISubject<Unit> activated;

        private readonly ViewCache cache;
        private readonly ViewLoader loader;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveShell"/> class.
        /// This class corresponds to the Frame class on Windows
        /// and the PhoneApplicationFrame class on Windows Phone.
        /// </summary>
        public ReactiveShell()
        {
            this.navigating = new Lazy<Subject<NavigatingInfo>>(() => new Subject<NavigatingInfo>());
            this.navigated = new Lazy<Subject<NavigatedInfo>>(() => new Subject<NavigatedInfo>());
            this.activated = new Subject<Unit>();

            this.BackStack = new ReactiveList<IJournalEntry>();
            this.ForwardStack = new ReactiveList<IJournalEntry>();

            this.cache = new ViewCache();
            this.loader = new ViewLoader(this.cache);
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            this.overlays = new List<Action<Panel>>();

            // we only want a single navigation active at a time.
            this.navigations = new SemaphoreSubject<Tuple<IJournalEntry, NavigationMode>>(1);
            this.isNavigationActive = new BehaviorSubject<bool>(false);
            this.journal = this.navigations
                .Do(_ => 
                { 
                    lock (isNavigationActive) 
                    { 
                        isNavigationActive.OnNext(true); 
                    } 
                })
                .SelectMany(tuple => this.NavigateToJournalEntry(tuple.Item1, tuple.Item2)
                    .Finally(() => 
                    {
                        this.navigations.Release();
                        lock (isNavigationActive) 
                        { 
                            isNavigationActive.OnNext(true); 
                        } 
                    })
                )
                .Publish().RefCount();
            //publishes the journal entry of the visible view
            this.CurrentJournalEntry = this.journal.Where(tuple => tuple.Item3).Select(tuple => tuple.Item1).Publish(null).RefCount();
            // publishes the visible view
            this.CurrentView = this.journal.Where(tuple => tuple.Item3).Select(tuple => tuple.Item2).Publish(null).RefCount();
            // publishes a value indicating if a navigation is active
            this.IsNavigationActive = this.isNavigationActive.DistinctUntilChanged();

            this.DefaultStyleKey = typeof(ReactiveShell);
        }

        #endregion

        #region Overrides

        /// <summary>
        /// When overridden in a derived class, is invoked whenever application code or internal processes (such as a rebuilding layout pass) call <see cref="M:System.Windows.Controls.Control.ApplyTemplate" />. In simplest terms, this means the method is called just before a UI element displays in an application. For more information, see Remarks.
        /// </summary>
#if WINDOWS_PHONE
        public
#else
        protected
#endif
 override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            viewPresentersPanel = (Panel)GetTemplateChild(ViewPresentersPanelName);
            if (viewPresentersPanel == null)
            {
                throw new InvalidOperationException("ReactiveShell template is missing PART_ViewPresentersPanel");
            }
            else
            {
                if (currentViewPresenter != null)
                {
                    viewPresentersPanel.Children.Add(currentViewPresenter);
                }
            }

            //add overlays if they are queued
            foreach (var func in this.overlays)
            {
                func(this.viewPresentersPanel);
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Navigates to the view asynchronous.
        /// </summary>
        /// <typeparam name="V">The type of the view which must inherit from ReactiveView.</typeparam>
        /// <param name="view">The view. If null, a new instance is optionally created based on the CacheMode parameter.</param>
        /// <returns></returns>
        public IObservable<bool> ViewAsync<V>(V view, NavigationMode mode, object parameter = null) where V : ReactiveView
        {
            //create journal entry based on arguments.
            return this.ViewAsync((IJournalEntry)new JournalEntry(typeof(V), parameter) { State = view }, mode);
        }

        public IObservable<bool> ViewAsync(IJournalEntry entry, NavigationMode mode)
        {
            // this is not entirely a safe check, because we in theory can have multiple navigation to the same 
            // journalentry scheduled after each other, but that is weird and will probably never happen
            IObservable<bool> result = this.journal.FirstOrDefaultAsync(tuple => tuple.Item1 == entry).Select(tuple => tuple.Item3);

            // schedule the navigation action asynchronous so we can return the observable
            RxApp.MainThreadScheduler.Schedule(() => this.navigations.OnNext(Tuple.Create(entry, mode)));

            return result;
        }

        /// <summary>
        /// Returns an observable of booleans indicating if we can navigate to a new view.
        /// </summary>
        /// <value>
        /// The observable of booleans.
        /// </value>
        public IObservable<bool> IsNavigationActive
        {
            get;
            private set;
        }

        public IObservable<NavigatingInfo> Navigating
        {
            get { return navigating.Value; }
        }

        public IObservable<NavigatedInfo> Navigated
        {
            get { return navigated.Value; }
        }

        #endregion

        #region Journal

        public IReactiveList<IJournalEntry> BackStack
        {
            get;
            private set;
        }

        public IReactiveList<IJournalEntry> ForwardStack
        {
            get;
            private set;
        }

        public IObservable<IJournalEntry> CurrentJournalEntry
        {
            get;
            private set;
        }

        public IObservable<ReactiveView> CurrentView
        {
            get;
            private set;
        }

        private bool disableJournal;
        public bool DisableJournal
        {
            get { return this.disableJournal; }
            set
            {
                this.disableJournal = value;
                if (!value)
                {
                    this.BackStack.Clear();
                    this.ForwardStack.Clear();
                }
            }
        }

        #endregion

        #region Activation

        public IObservable<Unit> Activated
        {
            get { return this.activated; }
        }

        public IObservable<Unit> Activate()
        {
#if !WINDOWS_PHONE
            Func<Unit> func = () =>
            {
                if (Window.Current.Content != this)
                {
                    Window.Current.Content = this;
                }
                Window.Current.Activate();
                //fire activated
                activated.OnNext(Unit.Default);
                return Unit.Default;
            };
            return Observable.Return<Unit>(func());
#else
            return Observable.Return<Unit>(Unit.Default);
#endif
        }

        #endregion

        public NavigationCacheMode NavigationCacheMode
        {
            get { return this.loader.DefaultCacheMode; }
            set { this.loader.DefaultCacheMode = value; }
        }

        /// <summary>
        /// Adds the overlay either to the panel or defers the adding.
        /// </summary>
        /// <param name="overlay">The overlay.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">overlay must be UIElement</exception>
        public IDisposable AddOverlay(object overlay)
        {
            UIElement element = overlay as UIElement;
            if (element == null)
            {
                throw new ArgumentException("overlay must be UIElement");
            }

            //The returning disposable needs to remove the overlay from the panel,
            // or remove the object from the list of overlay objects.
            SerialDisposable disp = new SerialDisposable();
            Action<Panel> addFunction = panel =>
            {
                panel.Children.Add(element);
                disp.Disposable = Disposable.Create(() => panel.Children.Remove(element));
            };
            this.overlays.Add(addFunction);
            if (this.viewPresentersPanel != null)
            {
                addFunction(this.viewPresentersPanel);
            }
            return new CompositeDisposable(disp, Disposable.Create(() => this.overlays.Remove(addFunction)));
        }

        private IObservable<Tuple<IJournalEntry, ReactiveView, bool>> NavigateToJournalEntry(IJournalEntry journalEntry, NavigationMode navigationMode)
        {
            return this.NavigateInternal(() => this.loader.GetView(journalEntry), journalEntry, navigationMode);
        }

        private IObservable<Tuple<IJournalEntry, ReactiveView, bool>> NavigateInternal(Func<ReactiveView> view, IJournalEntry journalEntry, NavigationMode navigationMode)
        {
            return Observable.Defer(() => this.CurrentJournalEntry.FirstOrDefaultAsync()).SelectMany(async currentJournalEntry =>
            {
                ReactiveView currentView = null;
                ReactiveView newView = null;
                ContentPresenter newViewPresenter;

                //disable frame interactions
                using (this.DisableFrameInteractivity())
                {
                    if (currentViewPresenter != null)
                    {
                        DisableContentPresenterInteractivity(currentViewPresenter);
                        currentView = (ReactiveView)currentViewPresenter.Content;
                    }

                    this.Log().Info("Navigating started.");

                    NavigatingInfo navigating = new NavigatingInfo(navigationMode, journalEntry);
                    if (!await this.PeformNavigating(currentView, navigating, () =>
                        {
                            this.Log().Info("Creating view.");
                            newView = view();
                            newView.Shell = this;
                            this.Log().Info("Created view {0}.", newView.GetType());
                            return newView;
                        }))
                    {
                        this.Log().Info("Navigating aborted.");
                        return Tuple.Create(journalEntry, (ReactiveView)null, false);
                    }
                    this.Log().Info("Navigating completed.");

                    newViewPresenter = new ContentPresenter();
                    newViewPresenter.Content = newView;

                    DisableContentPresenterInteractivity(newViewPresenter);

                    //add new page presenter
                    if (viewPresentersPanel != null)
                    {
                        viewPresentersPanel.Children.Add(newViewPresenter);
                    }

                    //do not update journal if disabled
                    if (!this.DisableJournal)
                    {
                        this.UpdateJournal(navigationMode, currentJournalEntry, journalEntry);
                    }

                    this.Log().Info("Navigated started.");
                    NavigatedInfo navigated = new NavigatedInfo(newView, navigationMode, journalEntry);
                    await this.PeformNavigated(currentView, navigated, newView);
                    this.Log().Info("Navigated completed.");

                    //remove old page presenter
                    if (viewPresentersPanel != null && currentViewPresenter != null)
                    {
                        viewPresentersPanel.Children.Remove(currentViewPresenter);
                        //detach the page from any other control to prevent exceptions because of already in visual tree
                        currentViewPresenter.Content = null;
                    }

                    currentViewPresenter = newViewPresenter;

                    RestoreContentPresenterInteractivity(newViewPresenter);

                    //return a tuple containing navigation information about the journal entry.
                    return Tuple.Create(journalEntry, newView, true);
                }
            }).LoggedCatch(this, Observable.Return(Tuple.Create(journalEntry, (ReactiveView)null, false)));
        }

        private async Task<bool> PeformNavigating(ReactiveView currentView, NavigatingInfo navigatingInfo, Func<ReactiveView> view)
        {
            //OnNavigating                
            if (this.navigating.IsValueCreated)
            {
                this.navigating.Value.OnNext(navigatingInfo);
                if (navigatingInfo.Cancel)
                {
                    return false;
                }
            }
            if (currentView != null)
            {
                await currentView.OnNavigatingFromInternalAsync(this, navigatingInfo);
                if (navigatingInfo.Cancel)
                {
                    return false;
                }
            }

            ReactiveView newView = view();

            //OnNavigatingTo
            await newView.OnNavigatingToInternalAsync(this, navigatingInfo);

            if (navigatingInfo.Cancel)
            {
                return false;
            }

            return true;
        }

        private async Task PeformNavigated(ReactiveView currentView, NavigatedInfo navigatedInfo, ReactiveView newView)
        {
            if (this.navigated.IsValueCreated)
            {
                this.navigated.Value.OnNext(navigatedInfo);
            }

            if (currentView != null)
            {
                await currentView.OnNavigatedFromInternalAsync(this, navigatedInfo);
            }

            if (newView != null)
            {
                await newView.OnNavigatedToInternalAsync(this, navigatedInfo);
            }
        }

        private void UpdateJournal(NavigationMode navigationMode, IJournalEntry currentJournalEntry, IJournalEntry newJournalEntry)
        {
            switch (navigationMode)
            {
                case NavigationMode.New:
                    //remove existing entry in forward stack
                    ((IList<IJournalEntry>)this.ForwardStack).Clear();

                    if (currentJournalEntry != null)
                    {
                        this.BackStack.Add(currentJournalEntry);
                    }

                    break;
                case NavigationMode.Forward:
                    ((IList<IJournalEntry>)this.ForwardStack).RemoveUntil(newJournalEntry);

                    if (currentJournalEntry != null)
                    {
                        this.BackStack.Add(currentJournalEntry);
                    }

                    break;
                case NavigationMode.Back:
                    ((IList<IJournalEntry>)this.BackStack).RemoveUntil(newJournalEntry);

                    if (currentJournalEntry != null)
                    {
                        this.ForwardStack.Add(currentJournalEntry);
                    }

                    break;
            }
        }

        #region Interactions

        private IDisposable DisableFrameInteractivity()
        {
            this.IsHitTestVisible = true;
            return Disposable.Create(() =>
            {
                this.IsHitTestVisible = false;
            });
        }

        private static void DisableContentPresenterInteractivity(ContentPresenter presenter, bool applyBitmapCache = true)
        {
            if (presenter == null)
            {
                return;
            }
            if (applyBitmapCache)
            {
                presenter.CacheMode = BitmapCacheMode;
            }
            presenter.IsHitTestVisible = false;
        }

        private static void RestoreContentPresenterInteractivity(ContentPresenter presenter)
        {
            if (presenter == null)
            {
                return;
            }
            presenter.CacheMode = null;
            if (presenter.Opacity != 1.0)
            {
                presenter.Opacity = 1.0;
            }
            presenter.IsHitTestVisible = true;
        }

        #endregion
    }
}
