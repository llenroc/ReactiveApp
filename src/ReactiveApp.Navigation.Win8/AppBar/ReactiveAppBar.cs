using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Subjects;
using Splat;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Animation;
using System.Windows.Media;
using System.ComponentModel;
#else
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
#endif

namespace ReactiveApp.Navigation
{
    [TemplatePart(Name = LayoutRootPartName, Type = typeof(FrameworkElement))]
    [TemplatePart(Name = ContentPartName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = MenuPartName, Type = typeof(ItemsControl))]
    [TemplatePart(Name = DotsPartName, Type = typeof(FrameworkElement))]
    public class ReactiveAppBar : ContentControl
    {
        #region Template Part and Visual State names

        private const string LayoutRootPartName = "PART_LayoutRoot";
        private const string ContentPartName = "PART_Content";
        private const string MenuPartName = "PART_Menu";
        private const string DotsPartName = "PART_Dots";

        #endregion

        #region Constants
        private static readonly Duration Immediately = new Duration(TimeSpan.Zero);
        private static readonly Duration EntranceDuration = new Duration(TimeSpan.FromMilliseconds(75.0));

        #endregion

        #region Fields

        private FrameworkElement layoutRoot;
        private ContentPresenter content;
        private ItemsControl items;
        private FrameworkElement dots;

        private int renderCount;

        private DoubleAnimator controlAnimator;
        private DoubleAnimator layoutRootAnimator;
        private EasingFunctionBase layoutRootEasingFunction;

        private ISubject<Unit> openedSubject;
        private ISubject<Unit> closedSubject;

        #endregion

        #region Dependency Properties

        #region PlacementMode (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  PlacementMode.  This enables animation, styling, binding, etc...    
        /// </summary>
        internal static readonly DependencyProperty PlacementModeProperty =
            DependencyProperty.Register(
                "PlacementMode",
                typeof(PlacementMode),
                typeof(ReactiveAppBar),
                new PropertyMetadata(PlacementMode.Bottom)
            );

        /// <summary>
        /// The placement mode.
        /// </summary>
        internal PlacementMode PlacementMode
        {
            get { return (PlacementMode)this.GetValue(PlacementModeProperty); }
            set { this.SetValue(PlacementModeProperty, value); }
        }

        #endregion

        #region Mode (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  Mode.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(
                "Mode",
                typeof(ReactiveAppBarMode),
                typeof(ReactiveAppBar),
                new PropertyMetadata(ReactiveAppBarMode.Hidden, new PropertyChangedCallback(OnModeChanged))
            );

        /// <summary>
        /// The mode the AppBar is in when it is not opened.
        /// </summary>
        public ReactiveAppBarMode Mode
        {
            get { return (ReactiveAppBarMode)this.GetValue(ModeProperty); }
            set { this.SetValue(ModeProperty, value); }
        }

        private static void OnModeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveAppBar)sender).OnModeChanged((ReactiveAppBarMode)e.OldValue, (ReactiveAppBarMode)e.NewValue);
        }

        /// <summary>
        /// Called when the Mode is changed.
        /// </summary>
        protected virtual void OnModeChanged(ReactiveAppBarMode oldMode, ReactiveAppBarMode newMode)
        {
        }

        #endregion        

        #region MenuItems (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  MenuItems.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(
                "MenuItems",
                typeof(IList),
                typeof(ReactiveAppBar),
                new PropertyMetadata(null, new PropertyChangedCallback(OnMenuItemsChanged))
            );

        /// <summary>
        /// The menu items for this AppBar.
        /// </summary>
        public IList MenuItems
        {
            get { return (IList)this.GetValue(MenuItemsProperty); }
            set { this.SetValue(MenuItemsProperty, value); }
        }

        private static void OnMenuItemsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveAppBar)sender).OnMenuItemsChanged((IList)e.OldValue, (IList)e.NewValue);
        }

        /// <summary>
        /// Called when the MenuItems is changed.
        /// </summary>
        protected virtual void OnMenuItemsChanged(IList oldMenuItems, IList newMenuItems)
        {
            
        }

        #endregion

        #region IsMenuEnabled (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  IsMenuEnabled.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty IsMenuEnabledProperty =
            DependencyProperty.Register(
                "IsMenuEnabled",
                typeof(bool),
                typeof(ReactiveAppBar),
                new PropertyMetadata(true, new PropertyChangedCallback(OnIsMenuEnabledChanged))
            );

        /// <summary>
        /// Determines if the menu items are visible or not.
        /// </summary>
        public bool IsMenuEnabled
        {
            get { return (bool)this.GetValue(IsMenuEnabledProperty); }
            set { this.SetValue(IsMenuEnabledProperty, value); }
        }

        private static void OnIsMenuEnabledChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveAppBar)sender).OnIsMenuEnabledChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Called when the IsMenuEnabled is changed.
        /// </summary>
        protected virtual void OnIsMenuEnabledChanged(bool oldIsMenuEnabled, bool newIsMenuEnabled)
        {
        }

        #endregion

        #region IsOpen (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  IsOpen.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register(
                "IsOpen",
                typeof(bool),
                typeof(ReactiveAppBar),
                new PropertyMetadata(false, new PropertyChangedCallback(OnIsOpenChanged))
            );

        /// <summary>
        /// Indicates if the appbar is open or not.
        /// </summary>
        public bool IsOpen
        {
            get { return (bool)this.GetValue(IsOpenProperty); }
            set { this.SetValue(IsOpenProperty, value); }
        }

        private static void OnIsOpenChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveAppBar)sender).OnIsOpenChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Called when the IsOpen is changed.
        /// </summary>
        protected virtual void OnIsOpenChanged(bool oldIsOpen, bool newIsOpen)
        {
        }

        #endregion

        #region CanOpen (Dependency Property)

        /// <summary>
        /// CanOpen Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanOpenProperty =
            DependencyProperty.Register(
                "CanOpen",
                typeof(bool),
                typeof(ReactiveAppBar),
                new PropertyMetadata(true, OnCanOpenChanged));

        /// <summary>
        /// Gets or sets the CanOpen property. This dependency property 
        /// indicates whether the AppBar can open using the standard gestures.
        /// If this is set to false the AppBar is hidden no matter what mode is set.
        /// </summary>
        public bool CanOpen
        {
            get { return (bool)GetValue(CanOpenProperty); }
            set { SetValue(CanOpenProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanOpen property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanOpenChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveAppBar)d).OnCanOpenChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanOpen property.
        /// </summary>
        /// <param name="oldCanOpen">The old CanOpen value</param>
        /// <param name="newCanOpen">The new CanOpen value</param>
        protected virtual void OnCanOpenChanged(bool oldCanOpen, bool newCanOpen)
        {
            if (!newCanOpen)
            {
                IsOpen = false;
            }
        }

        #endregion

        #region CanDismiss (Dependency Property)

        /// <summary>
        /// CanDismiss Dependency Property
        /// </summary>
        public static readonly DependencyProperty CanDismissProperty =
            DependencyProperty.Register(
                "CanDismiss",
                typeof(bool),
                typeof(ReactiveAppBar),
                new PropertyMetadata(true, OnCanDismissChanged));

        /// <summary>
        /// Gets or sets the CanDismiss property. This dependency property 
        /// indicates whether the AppBar can be dismissed (closed).
        /// </summary>
        public bool CanDismiss
        {
            get { return (bool)GetValue(CanDismissProperty); }
            set { SetValue(CanDismissProperty, value); }
        }

        /// <summary>
        /// Handles changes to the CanDismiss property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnCanDismissChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ReactiveAppBar)d).OnCanDismissChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes
        /// to the CanDismiss property.
        /// </summary>
        /// <param name="oldCanDismiss">The old CanDismiss value</param>
        /// <param name="newCanDismiss">The new CanDismiss value</param>
        protected virtual void OnCanDismissChanged(bool oldCanDismiss, bool newCanDismiss)
        {
            if (!newCanDismiss)
            {
                IsOpen = true;
            }
        }

        #endregion

        #region IsLightDismissEnabled (Dependency Property)

        /// <summary>
        /// IsLightDismissEnabled Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsLightDismissEnabledProperty =
            DependencyProperty.Register(
                "IsLightDismissEnabled",
                typeof(bool),
                typeof(ReactiveAppBar),
                new PropertyMetadata(true));

        /// <summary>
        /// Gets or sets the IsLightDismissEnabled property. This dependency property 
        /// indicates whether the app bar can be dismissed by tapping anywhere outside of the control.
        /// </summary>
        public bool IsLightDismissEnabled
        {
            get { return (bool)GetValue(IsLightDismissEnabledProperty); }
            set { SetValue(IsLightDismissEnabledProperty, value); }
        }

        #endregion

        #region ExpandedSize (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  ExpandedSize.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ExpandedSizeProperty =
            DependencyProperty.Register(
                "ExpandedSize",
                typeof(double),
                typeof(ReactiveAppBar),
                new PropertyMetadata(
#if !WINDOWS_PHONE
68.0
#else
72.0
#endif
)
            );

        /// <summary>
        /// Gets or sets the distance that the ReactiveAppBar extends into a page when the 
        /// <see cref="P:Methylium.Navigation.ReactiveAppBar.Mode"/> property is set to <see cref="F:Methylium.Navigation.ReactiveAppBarMode.Expanded"/>.
        /// </summary>
        /// <returns>
        /// The distance that the ReactiveAppBar extends into a page.
        /// </returns>
        public double ExpandedSize
        {
            get { return (double)this.GetValue(ExpandedSizeProperty); }
            set { this.SetValue(ExpandedSizeProperty, value); }
        }

        #endregion

        #region MinimizedSize (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  MinimizedSize.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty MinimizedSizeProperty =
            DependencyProperty.Register(
                "MinimizedSize",
                typeof(double),
                typeof(ReactiveAppBar),
                new PropertyMetadata(30.0)
            );


        /// <summary>
        /// Gets or sets the distance that the ReactiveAppBar extends into a page when the 
        /// <see cref="P:Methylium.Navigation.ReactiveAppBar.Mode"/> property is set to <see cref="F:Methylium.Navigation.ReactiveAppBarMode.Minimized"/>.
        /// </summary>
        /// <returns>
        /// The distance that the ReactiveAppBar extends into a page.
        /// </returns>
        public double MinimizedSize
        {
            get { return (double)this.GetValue(MinimizedSizeProperty); }
            set { this.SetValue(MinimizedSizeProperty, value); }
        }

        #endregion

        #region ShowDots (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  ShowDots.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ShowDotsProperty =
            DependencyProperty.Register(
                "ShowDots",
                typeof(bool),
                typeof(ReactiveAppBar),
                new PropertyMetadata(
#if WINDOWS_PHONE
true
#else
false
#endif
)
            );

        /// <summary>
        /// Indicates if the right top of the appbar contains dots.
        /// </summary>
        public bool ShowDots
        {
            get { return (bool)this.GetValue(ShowDotsProperty); }
            set { this.SetValue(ShowDotsProperty, value); }
        }

        #endregion
        
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveAppBar"/> class.
        /// </summary>
        public ReactiveAppBar()
        {
            Debug.WriteLine("ReactiveAppBar {0} created.", this.GetHashCode());

            this.DefaultStyleKey = typeof(ReactiveAppBar);

            this.openedSubject = new Subject<Unit>();
            this.closedSubject = new Subject<Unit>();

            this.layoutRootEasingFunction = new PowerEase() { EasingMode = EasingMode.EaseOut, Power = 1 };
            this.MenuItems = new List<Button>();

            this.Loaded += ReactiveAppBar_Loaded;
            this.Unloaded += ReactiveAppBar_Unloaded;
        }

        void ReactiveAppBar_Loaded(object sender, RoutedEventArgs e)
        {
            // make sure the template is available during the Loaded event.
            // on Windows Phone this is by default not guaranteed
            // http://msdn.microsoft.com/en-us/library/system.windows.frameworkelement.loaded(v=vs.95).aspx
            this.ApplyTemplate();
        }

        void ReactiveAppBar_Unloaded(object sender, RoutedEventArgs e)
        {
            renderCount = 0;
        }

        #region OnApplyTemplate()

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call ApplyTemplate. 
        /// In simplest terms, this means the method is called just before a UI element displays in your app. 
        /// Override this method to influence the default post-template logic of a class.
        /// </summary>
#if WINDOWS_PHONE
        public
#else
        protected
#endif
 override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.layoutRoot = GetTemplateChild(LayoutRootPartName) as FrameworkElement;
            if (this.layoutRoot == null)
            {
                throw new InvalidOperationException(string.Format("{0} is missing from template.", LayoutRootPartName));
            }
            this.content = GetTemplateChild(ContentPartName) as ContentPresenter;
            if (this.content == null)
            {
                throw new InvalidOperationException(string.Format("{0} is missing from template.", ContentPartName));
            }
            this.items = GetTemplateChild(MenuPartName) as ItemsControl;
            if (this.items == null)
            {
                throw new InvalidOperationException(string.Format("{0} is missing from template.", MenuPartName));
            }
            this.dots = GetTemplateChild(DotsPartName) as FrameworkElement;
            if (this.dots == null)
            {
                throw new InvalidOperationException(string.Format("{0} is missing from template.", DotsPartName));
            }

            this.InitializeAnimators();
            this.EvaluateMenuVisibility();
            this.EvaluateDotsVisibility();
        }

        #endregion

        internal void HookIntoRendering()
        {
            CompositionTarget.Rendering -= ReactiveAppBar_Rendering;
            CompositionTarget.Rendering += ReactiveAppBar_Rendering;
        }

        /// <summary>
        /// Occurs when the rendering process renders a frame.
        /// Used to play the entrance animation of the appbar.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        void ReactiveAppBar_Rendering(object sender, object e)
        {
            if (layoutRootAnimator != null)
            {

#if WINDOWS_PHONE
                if (!DesignerProperties.IsInDesignTool)
#else
                if (!DesignMode.DesignModeEnabled)
#endif
                {
                    switch (renderCount++)
                    {

                        case 0:
                            //don't play entrance animation
                            if (this.Mode == ReactiveAppBarMode.Hidden)
                            {
                                CompositionTarget.Rendering -= ReactiveAppBar_Rendering;
                            }
                            this.GoToMode(ReactiveAppBarMode.Hidden, ReactiveAppBar.Immediately);
                            break;
                        case 1:
                            CompositionTarget.Rendering -= ReactiveAppBar_Rendering;
                            this.GoToMode(this.Mode, ReactiveAppBar.EntranceDuration, null);
                            break;
                    }
                }
                else
                {
                    CompositionTarget.Rendering -= ReactiveAppBar_Rendering;
                    this.GoToMode(this.Mode, ReactiveAppBar.Immediately);
                }
            }
        }

        private void EvaluateMenuVisibility()
        {
            this.items.Visibility = this.IsMenuEnabled ? Visibility.Visible : Visibility.Collapsed;
        }

        private void EvaluateDotsVisibility()
        {
            this.dots.Visibility = this.ShowDots ? Visibility.Visible : Visibility.Collapsed;
        }

        #region GoToMode()

        private void GoToMode(ReactiveAppBarMode mode, Duration duration, Action onCompleted = null)
        {
            if (layoutRootAnimator != null)
            {
                double offset = GetModeOffset(mode);
                this.layoutRootAnimator.GoTo(offset, duration, layoutRootEasingFunction, onCompleted);
            }
        }
        #endregion

        #region GoToOpened()

        private void GoToOpened(Duration duration, Action onCompleted = null)
        {
            if (layoutRootAnimator != null)
            {
                this.layoutRootAnimator.GoTo(0, duration, layoutRootEasingFunction, onCompleted);
            }
        }

        #endregion

        private double GetModeOffset(ReactiveAppBarMode mode)
        {
            switch (mode)
            {
                case ReactiveAppBarMode.Minimized:
                    double minimizedSize = MinimizedSize;
                    switch (this.PlacementMode)
                    {
                        case PlacementMode.Left:
                            return -(this.ActualWidth - minimizedSize);
                        case PlacementMode.Right:
                            return (this.ActualWidth - minimizedSize);
                        case PlacementMode.Bottom:
                            return (this.ActualHeight - minimizedSize);
                        case PlacementMode.Top:
                            return -(this.ActualHeight - minimizedSize);
                        default:
                            break;
                    }
                    break;
                case ReactiveAppBarMode.Expanded:
                    double expandedSize = ExpandedSize;
                    switch (this.PlacementMode)
                    {
                        case PlacementMode.Left:
                            return -(this.ActualWidth - expandedSize);
                        case PlacementMode.Right:
                            return (this.ActualWidth - expandedSize);
                        case PlacementMode.Bottom:
                            return (this.ActualHeight - expandedSize);
                        case PlacementMode.Top:
                            return -(this.ActualHeight - expandedSize);
                        default:
                            break;
                    }
                    break;
                case ReactiveAppBarMode.Hidden:
                default:
                    switch (this.PlacementMode)
                    {
                        case PlacementMode.Left:
                            return -this.ActualWidth;
                        case PlacementMode.Right:
                            return this.ActualWidth;
                        case PlacementMode.Bottom:
                            return this.ActualHeight;
                        case PlacementMode.Top:
                            return -this.ActualHeight;
                        default:
                            break;
                    }
                    break;
            }
            return 0;
        }
        
        private void InitializeAnimators()
        {
            this.layoutRoot.RenderTransform = new TranslateTransform();
            this.RenderTransform = new TranslateTransform();
            string property = "Y";

            switch (this.PlacementMode)
            {
                case PlacementMode.Left:
                case PlacementMode.Right:
                    property = "X";
                    break;
            }

            this.layoutRootAnimator = new DoubleAnimator(this.layoutRoot.RenderTransform, property);
            this.controlAnimator = new DoubleAnimator(this.RenderTransform, property);
        }

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

            return factory.GetLogger<ReactiveAppBar>();
        }

        #region Properties

        /// <summary>
        /// Occurs when back button is pressed.
        /// </summary>
        public IObservable<Unit> Opened
        {
            get { return this.openedSubject; }
        }

        /// <summary>
        /// Occurs when back button is pressed.
        /// </summary>
        public IObservable<Unit> Closed
        {
            get { return this.closedSubject; }
        }

        #endregion

#if DEBUG
        ~ReactiveAppBar()
        {
            string debug = string.Format("ReactiveAppBar {0} finalised.", this.GetHashCode());
            Debug.WriteLine(debug);
            this.Log().Debug(debug);
        }
#endif
    }
}
