using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Splat;

#if WINDOWS_PHONE
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls.Primitives;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
#endif

namespace ReactiveApp.Navigation
{
    /// <summary>
    /// Class similar to the Page class on Windows and PhoneApplicationPage on Windows Phone.
    /// 
    /// Several events occur on this class in the following order:
    /// 
    /// Windows:        Constructor, OnNavigatingToAsync, OnNavigatedToAsync, Loaded
    /// Windows Phone:  Constructor, OnNavigatingToAsync, OnNavigatedToAsync, Loaded 
    /// 
    /// </summary>
    public class ReactiveView : UserControl
    {
        private Binding dataContextBinding;
        private IObservable<Unit> completed;
        private bool areAppBarsregistered = false;

        private IDisposable topAppBarDisposable = Disposable.Empty;
        private IDisposable rightAppBarDisposable = Disposable.Empty;
        private IDisposable leftAppBarDisposable = Disposable.Empty;
        private IDisposable bottomAppBarDisposable = Disposable.Empty;

        #region Dependency Properties

        #region Shell (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for the ShellProperty. This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ShellProperty =
            DependencyProperty.Register(
                "Shell",
                typeof(ReactiveShell),
                typeof(ReactiveView),
                new PropertyMetadata(null, new PropertyChangedCallback(OnShellChanged))
            );

        /// <summary>
        /// Gets the frame where this page is displayed.
        /// </summary>
        public ReactiveShell Shell
        {
            get { return (ReactiveShell)this.GetValue(ShellProperty); }
            set { this.SetValue(ShellProperty, value); }
        }

        private static void OnShellChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveView)sender).OnShellChanged((ReactiveShell)e.OldValue, (ReactiveShell)e.NewValue);
        }

        /// <summary>
        /// Called when the Frame is changed.
        /// </summary>
        protected virtual void OnShellChanged(ReactiveShell oldFrame, ReactiveShell newFrame)
        {
        }

        #endregion

        #region TopAppBar (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  TopAppBar.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty TopAppBarProperty =
            DependencyProperty.Register(
                "TopAppBar",
                typeof(ReactiveAppBar),
                typeof(ReactiveView),
                new PropertyMetadata(null, new PropertyChangedCallback(OnTopAppBarChanged))
            );

        /// <summary>
        /// The top application bar.
        /// </summary>
        public ReactiveAppBar TopAppBar
        {
            get { return (ReactiveAppBar)this.GetValue(TopAppBarProperty); }
            set { this.SetValue(TopAppBarProperty, value); }
        }

        private static void OnTopAppBarChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveView)sender).OnTopAppBarChanged((ReactiveAppBar)e.OldValue, (ReactiveAppBar)e.NewValue);
        }

        /// <summary>
        /// Called when the TopAppBar is changed.
        /// </summary>
        protected virtual void OnTopAppBarChanged(ReactiveAppBar oldTopAppBar, ReactiveAppBar newTopAppBar)
        {
            topAppBarDisposable.Dispose();
            if (oldTopAppBar != null)
            {
                oldTopAppBar.DataContext = null;
            }
            if (newTopAppBar != null)
            {
                newTopAppBar.PlacementMode = PlacementMode.Top;
                newTopAppBar.HorizontalAlignment = HorizontalAlignment.Stretch;
                newTopAppBar.VerticalAlignment = VerticalAlignment.Top;
                if (newTopAppBar.DataContext == null)
                {
                    newTopAppBar.SetBinding(ReactiveAppBar.DataContextProperty, dataContextBinding);
                }
                if (areAppBarsregistered)
                {
                    topAppBarDisposable = ReactiveAppBarManager.Instance.AddAppBar(newTopAppBar);
                }
            }
        }

        #endregion

        #region RightAppBar (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  RightAppBar.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty RightAppBarProperty =
            DependencyProperty.Register(
                "RightAppBar",
                typeof(ReactiveAppBar),
                typeof(ReactiveView),
                new PropertyMetadata(null, new PropertyChangedCallback(OnRightAppBarChanged))
            );

        /// <summary>
        /// The right application bar.
        /// </summary>
        public ReactiveAppBar RightAppBar
        {
            get { return (ReactiveAppBar)this.GetValue(RightAppBarProperty); }
            set { this.SetValue(RightAppBarProperty, value); }
        }

        private static void OnRightAppBarChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveView)sender).OnRightAppBarChanged((ReactiveAppBar)e.OldValue, (ReactiveAppBar)e.NewValue);
        }

        /// <summary>
        /// Called when the RightAppBar is changed.
        /// </summary>
        protected virtual void OnRightAppBarChanged(ReactiveAppBar oldRightAppBar, ReactiveAppBar newRightAppBar)
        {
            rightAppBarDisposable.Dispose();
            if (oldRightAppBar != null)
            {
                oldRightAppBar.DataContext = null;
            }
            if (newRightAppBar != null)
            {
                newRightAppBar.PlacementMode = PlacementMode.Right;
                newRightAppBar.HorizontalAlignment = HorizontalAlignment.Right;
                newRightAppBar.VerticalAlignment = VerticalAlignment.Stretch;
                if (newRightAppBar.DataContext == null)
                {
                    newRightAppBar.SetBinding(ReactiveAppBar.DataContextProperty, dataContextBinding);
                }
                if (this.areAppBarsregistered)
                {
                    rightAppBarDisposable = ReactiveAppBarManager.Instance.AddAppBar(newRightAppBar);
                }
            }
        }

        #endregion

        #region BottomAppBar (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  BottomAppBar.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty BottomAppBarProperty =
            DependencyProperty.Register(
                "BottomAppBar",
                typeof(ReactiveAppBar),
                typeof(ReactiveView),
                new PropertyMetadata(null, new PropertyChangedCallback(OnBottomAppBarChanged))
            );

        /// <summary>
        /// The bottom application bar.
        /// </summary>
        public ReactiveAppBar BottomAppBar
        {
            get { return (ReactiveAppBar)this.GetValue(BottomAppBarProperty); }
            set { this.SetValue(BottomAppBarProperty, value); }
        }

        private static void OnBottomAppBarChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveView)sender).OnBottomAppBarChanged((ReactiveAppBar)e.OldValue, (ReactiveAppBar)e.NewValue);
        }

        /// <summary>
        /// Called when the BottomAppBar is changed.
        /// </summary>
        protected virtual void OnBottomAppBarChanged(ReactiveAppBar oldBottomAppBar, ReactiveAppBar newBottomAppBar)
        {
            bottomAppBarDisposable.Dispose();
            if (oldBottomAppBar != null)
            {
                oldBottomAppBar.DataContext = null;
            }
            if (newBottomAppBar != null)
            {
                newBottomAppBar.PlacementMode = PlacementMode.Bottom;
                newBottomAppBar.HorizontalAlignment = HorizontalAlignment.Stretch;
                newBottomAppBar.VerticalAlignment = VerticalAlignment.Bottom;
                if (newBottomAppBar.DataContext == null)
                {
                    newBottomAppBar.SetBinding(ReactiveAppBar.DataContextProperty, dataContextBinding);
                }
                if (this.areAppBarsregistered)
                {
                    bottomAppBarDisposable = ReactiveAppBarManager.Instance.AddAppBar(newBottomAppBar);
                }
            }
        }

        #endregion

        #region LeftAppBar (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  LeftAppBar.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty LeftAppBarProperty =
            DependencyProperty.Register(
                "LeftAppBar",
                typeof(ReactiveAppBar),
                typeof(ReactiveView),
                new PropertyMetadata(null, new PropertyChangedCallback(OnLeftAppBarChanged))
            );

        /// <summary>
        /// The left application bar.
        /// </summary>
        public ReactiveAppBar LeftAppBar
        {
            get { return (ReactiveAppBar)this.GetValue(LeftAppBarProperty); }
            set { this.SetValue(LeftAppBarProperty, value); }
        }

        private static void OnLeftAppBarChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveView)sender).OnLeftAppBarChanged((ReactiveAppBar)e.OldValue, (ReactiveAppBar)e.NewValue);
        }

        /// <summary>
        /// Called when the LeftAppBar is changed.
        /// </summary>
        protected virtual void OnLeftAppBarChanged(ReactiveAppBar oldLeftAppBar, ReactiveAppBar newLeftAppBar)
        {
            leftAppBarDisposable.Dispose();
            if (oldLeftAppBar != null)
            {
                oldLeftAppBar.DataContext = null;
            }
            if (newLeftAppBar != null)
            {
                newLeftAppBar.PlacementMode = PlacementMode.Left;
                newLeftAppBar.HorizontalAlignment = HorizontalAlignment.Left;
                newLeftAppBar.VerticalAlignment = VerticalAlignment.Stretch;
                if (newLeftAppBar.DataContext == null)
                {
                    newLeftAppBar.SetBinding(ReactiveAppBar.DataContextProperty, dataContextBinding);
                }
                if (this.areAppBarsregistered)
                {
                    leftAppBarDisposable = ReactiveAppBarManager.Instance.AddAppBar(newLeftAppBar);
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveView"/> class.
        /// </summary>
        public ReactiveView()
        {
            dataContextBinding = new Binding() { Source = this, Path = new PropertyPath("DataContext") };
            completed = Observable.Return<Unit>(Unit.Default);

            // this does not leak because the event is on the object itself
            // and therefore only references itself
            this.Loaded += ReactiveView_Loaded;
        }

        void ReactiveView_Loaded(object sender, RoutedEventArgs e)
        {
            // make sure the template is available during the Loaded event.
            // on Windows Phone this is by default not guaranteed
            // http://msdn.microsoft.com/en-us/library/system.windows.frameworkelement.loaded(v=vs.95).aspx
            this.ApplyTemplate();
        }

        #endregion

        #region Navigation

        internal IObservable<Unit> OnNavigatingFromInternalAsync(ReactiveShell shell, NavigatingInfo e)
        {
            return OnNavigatingFromAsync(e);
        }

        /// <summary>
        /// Invoked immediately before the Page is unloaded and is no longer the current
        /// source of a parent Frame.
        /// </summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative
        /// of the navigation that will unload the current Page unless canceled. The
        /// navigation can potentially be canceled by setting Cancel.
        /// </param>
        /// <returns></returns>
        protected virtual IObservable<Unit> OnNavigatingFromAsync(NavigatingInfo e)
        {
            return completed;
        }

        internal IObservable<Unit> OnNavigatingToInternalAsync(ReactiveShell shell, NavigatingInfo e)
        {
            return OnNavigatingToAsync(e);
        }

        /// <summary>
        /// Invoked immediately before the Page is loaded and is the current
        /// source of a parent Frame.
        /// </summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative
        /// of the navigation that will load the current Page unless canceled. The
        /// navigation can potentially be canceled by setting Cancel, but it is recommended
        /// to cancel earlier before the page is actually created. Use with care!
        /// </param>
        /// <returns></returns>
        protected virtual IObservable<Unit> OnNavigatingToAsync(NavigatingInfo e)
        {
            return completed;
        }

        internal IObservable<Unit> OnNavigatedFromInternalAsync(ReactiveShell shell, NavigatedInfo e)
        {
            this.UnregisterAppBars();
            return OnNavigatedFromAsync(e);
        }

        /// <summary>
        /// Invoked immediately after the Page is unloaded and is no longer the current
        /// source of a parent Frame.
        /// </summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative
        /// of the navigation that has unloaded the current Page.
        /// </param>
        /// <returns></returns>
        protected virtual IObservable<Unit> OnNavigatedFromAsync(NavigatedInfo e)
        {
            return completed;
        }

        internal IObservable<Unit> OnNavigatedToInternalAsync(ReactiveShell shell, NavigatedInfo e)
        {
            this.RegisterAppBars();
            return OnNavigatedToAsync(e);
        }

        /// <summary>
        /// Invoked when the Page is loaded and becomes the current source of a parent Frame.
        /// </summary>
        /// <param name="e">
        /// Event data that can be examined by overriding code. The event data is representative
        /// of the pending navigation that will load the current Page. Usually the most
        /// relevant property to examine is Parameter.
        /// </param>
        /// <returns></returns>
        protected virtual IObservable<Unit> OnNavigatedToAsync(NavigatedInfo e)
        {
            return completed;
        }

        #endregion

        /// <summary>
        /// The designer does not seem to like interfaces so we just implement this method directly instead of via extension methods on IEnableLogger.
        /// Drawback: We dont get the strongly typed class but its is better than nothing.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.Exception">ILogManager is null. This should never happen, your dependency resolver is broken</exception>
        private IFullLogger Log()
        {
            var factory = Locator.Current.GetService<ILogManager>();
            if (factory == null)
            {
                throw new Exception("ILogManager is null. This should never happen, your dependency resolver is broken");
            }

            return factory.GetLogger<ReactiveView>();
        }

        private void RegisterAppBars()
        {
            this.topAppBarDisposable = this.TopAppBar != null ? ReactiveAppBarManager.Instance.AddAppBar(this.TopAppBar) : Disposable.Empty;
            this.rightAppBarDisposable = this.RightAppBar != null ? ReactiveAppBarManager.Instance.AddAppBar(this.RightAppBar) : Disposable.Empty;
            this.bottomAppBarDisposable = this.BottomAppBar != null ? ReactiveAppBarManager.Instance.AddAppBar(this.BottomAppBar) : Disposable.Empty;
            this.leftAppBarDisposable = this.LeftAppBar != null ? ReactiveAppBarManager.Instance.AddAppBar(this.LeftAppBar) : Disposable.Empty;
            this.areAppBarsregistered = true;
        }

        private void UnregisterAppBars()
        {
            this.areAppBarsregistered = false;
            this.topAppBarDisposable.Dispose();
            this.rightAppBarDisposable.Dispose();
            this.leftAppBarDisposable.Dispose();
            this.bottomAppBarDisposable.Dispose();
        }

#if DEBUG
        ~ReactiveView()
        {
            string debug = string.Format("ReactiveView {0} finalised.", this.GetHashCode());
            Debug.WriteLine(debug);
            this.Log().Debug(debug);
        }
#endif
    }
}
