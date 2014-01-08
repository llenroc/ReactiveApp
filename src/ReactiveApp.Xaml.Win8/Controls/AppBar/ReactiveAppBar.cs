using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Splat;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
#endif

namespace ReactiveApp.Xaml.Controls
{
    public class ReactiveAppBar : ContentControl
    {
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
        
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="ReactiveAppBar"/> class.
        /// </summary>
        public ReactiveAppBar()
        {
            this.DefaultStyleKey = typeof(ReactiveAppBar);
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
