using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;

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

namespace ReactiveApp.Xaml.Controls
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
    public class ReactiveView : UserControl, IView<ReactiveShell, ReactiveView>
    {
        private Binding dataContextBinding;
        private IObservable<Unit> completed;

        #region Dependency Properties

        #region Shell (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for the ShellProperty. This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ShellProperty =
            DependencyProperty.Register(
                "Frame",
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

        #region PageTransitions

        /// <summary>
        /// PageTransition Dependency Property
        /// </summary>
        public static readonly DependencyProperty TransitionsProperty =
            DependencyProperty.Register(
                "Transitions",
                typeof(ViewTransitions),
                typeof(ReactiveView),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the PageTransition property. This dependency property 
        /// indicates the PageTransition to use to transition to this page.
        /// </summary>
        public ViewTransitions Transitions
        {
            get { return (ViewTransitions)GetValue(TransitionsProperty); }
            set { SetValue(TransitionsProperty, value); }
        }

        #endregion

        #region NavigationCacheMode (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for NavigationCacheMode.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty NavigationCacheModeProperty =
            DependencyProperty.Register(
                "NavigationCacheMode",
                typeof(NavigationCacheMode),
                typeof(ReactiveView),
                new PropertyMetadata(NavigationCacheMode.Inherit)
            );

        /// <summary>
        /// Specifies the cache mode for the current page. If not defined uses the 
        /// default cache mode defined on the frame.
        /// </summary>
        public NavigationCacheMode NavigationCacheMode
        {
            get { return (NavigationCacheMode)this.GetValue(NavigationCacheModeProperty); }
            set { this.SetValue(NavigationCacheModeProperty, value); }
        }

        #endregion

        #endregion

        #region Constructors

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

        internal IObservable<Unit> OnNavigatedFromInternalAsync(ReactiveShell frame, NavigatedInfo e)
        {
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

        internal IObservable<Unit> OnNavigatedToInternalAsync(ReactiveShell frame, NavigatedInfo e)
        {
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
    }
}
