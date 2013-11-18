using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

using ReactiveApp.Interfaces;
using ReactiveUI;

#if WINDOWS_PHONE
using System.Windows.Controls;
using System.Windows.Media;
#else
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#endif

namespace ReactiveApp.Xaml.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class ReactiveShell : ContentControl, IShell<ReactiveShell, ReactiveView>
    {
        #region Fields

        private static readonly CacheMode BitmapCacheMode = (CacheMode)new BitmapCache();
        private const string ViewPresentersPanelName = "PART_ViewPresentersPanel";
        private ContentPresenter currentViewPresenter;
        private Panel viewPresentersPanel;

        private readonly BehaviorSubject<IJournalEntry> viewJournal;
        private readonly ISubject<ReactiveView> view;
        private readonly ISubject<bool> canView;
        private readonly Lazy<Subject<NavigatingInfo>> navigating;
        private readonly Lazy<Subject<NavigatedInfo>> navigated;

        private readonly IObservable<bool> canBackView;
        private readonly IObservable<bool> canForwardView;

        private ViewCache cache;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveShell"/> class.
        /// This class corresponds to the Frame class on Windows
        /// and the PhoneApplicationFrame class on Windows Phone.
        /// </summary>
        public ReactiveShell()
        {
            this.viewJournal = new BehaviorSubject<IJournalEntry>(null);
            this.view = new BehaviorSubject<ReactiveView>(null);
            this.canView = new BehaviorSubject<bool>(true);
            this.navigating = new Lazy<Subject<NavigatingInfo>>(() => new Subject<NavigatingInfo>());
            this.navigated = new Lazy<Subject<NavigatedInfo>>(() => new Subject<NavigatedInfo>());

            // cast needed to access Count
            this.canBackView = Observable.CombineLatest(
                                    this.CanView,
                                    this.BackStack.Changed
                                        .Select(_ => ((ICollection<IJournalEntry>)this.BackStack).Count > 0)
                                        .StartWith(((ICollection<IJournalEntry>)this.BackStack).Count > 0),
                                    (b1, b2) => b1 && b2)
                                .DistinctUntilChanged()
                                .Publish(false)
                                .RefCount();
            this.canForwardView = Observable.CombineLatest(
                                    this.CanView,
                                    this.ForwardStack.Changed
                                        .Select(_ => ((ICollection<IJournalEntry>)this.ForwardStack).Count > 0)
                                        .StartWith(((ICollection<IJournalEntry>)this.ForwardStack).Count > 0),
                                    (b1, b2) => b1 && b2)
                                .DistinctUntilChanged()
                                .Publish(false)
                                .RefCount();

            this.cache = new ViewCache();
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
                throw new InvalidOperationException("ReactiveFrame template is missing PART_ViewPresentersPanel");
            }
            else
            {
                if (currentViewPresenter != null)
                {
                    viewPresentersPanel.Children.Add(currentViewPresenter);
                }
                //if (designGrid != null)
                //{
                //    pagePresentersPanel.Children.Add(designGrid);
                //}
            }
        }

        #endregion

        #region Navigation

        /// <summary>
        /// Navigates back to the view on the top of the backstack asynchronous.
        /// </summary>
        /// <typeparam name="V">The type of the view which must inherit from ReactiveView.</typeparam>
        /// <param name="view">The view. If null, a new instance is optionally created based on the CacheMode parameter.</param>
        /// <returns></returns>
        public IObservable<bool> BackViewAsync<V>(V view, object parameter = null) where V : ReactiveView
        {
            return this.CanBackView.FirstOrDefaultAsync()
                .Do(_ => { lock (this.canView) { this.canView.OnNext(false); } })
                .SelectMany(allowed =>
                {
                    if (allowed)
                    {
                        return this.NavigateToView(view, parameter, NavigationMode.Back, true);
                    }
                    else
                    {
                        return Observable.Throw<bool>(new InvalidOperationException("Can not navigate back at this time. Check CanBackView."));
                    }
                })
                .Do(_ => { lock (this.canView) { this.canView.OnNext(true); } });
        }

        public Task BackViewAsync(IJournalEntry entry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Navigates to the view asynchronous.
        /// </summary>
        /// <typeparam name="V">The type of the view which must inherit from ReactiveView.</typeparam>
        /// <param name="view">The view. If null, a new instance is optionally created based on the CacheMode parameter.</param>
        /// <returns></returns>
        public IObservable<bool> ViewAsync<V>(V view, object parameter = null) where V : ReactiveView
        {
            return this.CanView.FirstOrDefaultAsync()
                .Do(_ => { lock (this.canView) { this.canView.OnNext(false); } })
                .SelectMany(allowed =>
                {
                    if (allowed)
                    {
                        return this.NavigateToView(view, parameter, NavigationMode.New, true);
                    }
                    else
                    {
                        return Observable.Throw<bool>(new InvalidOperationException("Can not navigate at this time. Check CanView."));
                    }
                })
                .Do(_ => { lock (this.canView) { this.canView.OnNext(true); } });
        }

        public Task ViewAsync(IJournalEntry entry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Navigates forward to the view on the top of the forwardstack asynchronous.
        /// </summary>
        /// <typeparam name="V">The type of the view which must inherit from ReactiveView.</typeparam>
        /// <param name="view">The view. If null, a new instance is optionally created based on the CacheMode parameter.</param>
        /// <returns></returns>
        public IObservable<bool> ForwardViewAsync<V>(V view, object parameter = null) where V : ReactiveView
        {
            return this.CanForwardView.FirstOrDefaultAsync().ObserveOnDispatcher()
                .Do(_ => { lock (this.canView) { this.canView.OnNext(false); } })
                .SelectMany(allowed =>
                {
                    if (allowed)
                    {
                        return this.NavigateToView(view, parameter, NavigationMode.Forward, true);
                    }
                    else
                    {
                        return Observable.Throw<bool>(new InvalidOperationException("Can not navigate forward at this time. Check CanForwardView."));
                    }
                })
                .Do(_ => { lock (this.canView) { this.canView.OnNext(true); } });
        }

        public Task ForwardViewAsync(IJournalEntry entry)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns an observable of booleans indicating if we can go back to a previous view on the backstack.
        /// </summary>
        /// <value>
        /// The observable of booleans.
        /// </value>
        public IObservable<bool> CanBackView
        {
            get { return this.canBackView; }
        }

        /// <summary>
        /// Returns an observable of booleans indicating if we can navigate to a new view.
        /// </summary>
        /// <value>
        /// The observable of booleans.
        /// </value>
        public IObservable<bool> CanView
        {
            get { return this.canView; }
        }

        /// <summary>
        /// Returns an observable of booleans indicating if we can go forward to a previous view on the forwardstack.
        /// </summary>
        /// <value>
        /// The observable of booleans.
        /// </value>
        public IObservable<bool> CanForwardView
        {
            get { return this.canForwardView; }
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

        public IObservable<IJournalEntry> ViewJournal
        {
            get { return this.viewJournal; }
        }

        public IObservable<ReactiveView> View
        {
            get { return this.view; }
        }

        public bool DisableJournal
        {
            get;
            set;
        }

        #endregion

        #region Activation

        public IObservable<object> Activated
        {
            get { throw new NotImplementedException(); }
        }

        public Task Activate()
        {
            throw new NotImplementedException();
        }

        #endregion

        private IObservable<bool> NavigateToJournalEntry(IJournalEntry journalEntry, NavigationMode navigationMode, bool isNavigationInitiator)
        {
            return this.NavigateInternal(() => view, journalEntry, navigationMode, isNavigationInitiator);
        }

        private IObservable<bool> NavigateToView(ReactiveView view, object parameter, NavigationMode navigationMode, bool isNavigationInitiator)
        {
            return this.NavigateInternal(() => view, new JournalEntry(view.GetType(), parameter), navigationMode, isNavigationInitiator);
        }

        private IObservable<bool> NavigateInternal(Func<ReactiveView> view, IJournalEntry journalEntry, NavigationMode navigationMode, bool isNavigationInitiator)
        {
            return this.ViewJournal.SelectMany(async je =>
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

                    NavigatingInfo navigating = new NavigatingInfo(navigationMode, journalEntry, isNavigationInitiator, isNavigationInitiator);
                    if (!await this.PeformNavigating(currentView, navigating, () => { newView = view(); return newView; }))
                    {
                        return false;
                    }

                    newViewPresenter = new ContentPresenter();
                    newViewPresenter.Content = newView;

                    DisableContentPresenterInteractivity(newViewPresenter);

                    //add new page presenter
                    if (viewPresentersPanel != null)
                    {
                        viewPresentersPanel.Children.Add(newViewPresenter);
                    }

                    this.UpdateJournal(navigationMode, je);

                    this.viewJournal.OnNext(journalEntry);
                    this.view.OnNext(newView);

                    NavigatedInfo navigated = new NavigatedInfo(newView, navigationMode, journalEntry, isNavigationInitiator);
                    await this.PeformNavigated(currentView, navigated, newView);


                    //remove old page presenter
                    if (viewPresentersPanel != null && currentViewPresenter != null)
                    {
                        viewPresentersPanel.Children.Remove(currentViewPresenter);
                        //detach the page from any other control to prevent exceptions because of already in visual tree
                        currentViewPresenter.Content = null;
                    }

                    currentViewPresenter = newViewPresenter;

                    RestoreContentPresenterInteractivity(newViewPresenter);

                    return true;
                }
            });
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

        private void UpdateJournal(NavigationMode navigationMode, IJournalEntry currentJournalEntry)
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
                    ((IList<IJournalEntry>)this.ForwardStack).RemoveAt(((IList<IJournalEntry>)this.ForwardStack).Count - 1);

                    if (currentJournalEntry != null)
                    {
                        this.BackStack.Add(currentJournalEntry);
                    }

                    break;
                case NavigationMode.Back:
                    ((IList<IJournalEntry>)this.BackStack).RemoveAt(((IList<IJournalEntry>)this.BackStack).Count - 1);

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
