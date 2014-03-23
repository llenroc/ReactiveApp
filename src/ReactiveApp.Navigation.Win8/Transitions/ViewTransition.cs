

#if WINDOWS_PHONE
using System.Windows;
using System.Windows.Media.Animation;
#else
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
#endif

namespace ReactiveApp.Navigation
{
    /// <summary>
    /// Transition factory for a particular transition family.
    /// </summary>
    public abstract class ViewTransition : DependencyObject
    {
        public abstract Storyboard GetTransitionStoryboard(UIElement element);
    }
}
