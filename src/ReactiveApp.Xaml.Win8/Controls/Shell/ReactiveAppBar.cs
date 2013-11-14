using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE
using System.Windows.Controls;
using System.Windows;
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
    }
}
