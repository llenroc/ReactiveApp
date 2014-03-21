using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Media.Animation;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
#endif

namespace ReactiveApp.Xaml.Controls
{
    /// <summary>
    /// Transition factory for a particular transition family.
    /// </summary>
    public abstract class ViewTransition : DependencyObject
    {
        public abstract Storyboard GetTransitionStoryboard(UIElement element);
    }
}
