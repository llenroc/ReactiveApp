#if WINDOWS_PHONE
using System.Windows;
#else
using Windows.UI.Xaml;
#endif

namespace ReactiveApp.Navigation
{
    public class ViewTransitions : DependencyObject
    {
        #region ForwardOutTransition

        /// <summary>
        /// ForwardOutAnimation Dependency Property
        /// </summary>
        public static readonly DependencyProperty ForwardOutTransitionProperty =
            DependencyProperty.Register(
                "ForwardOutTransition",
                typeof(ViewTransition),
                typeof(ViewTransitions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the ForwardOutAnimation property. This dependency property 
        /// indicates the animation to use during forward navigation to remove the previous page from view.
        /// </summary>
        public ViewTransition ForwardOutTransition
        {
            get { return (ViewTransition)this.GetValue(ForwardOutTransitionProperty); }
            set { this.SetValue(ForwardOutTransitionProperty, value); }
        }
        #endregion

        #region ForwardInTransition
        /// <summary>
        /// ForwardInAnimation Dependency Property
        /// </summary>
        public static readonly DependencyProperty ForwardInTransitionProperty =
            DependencyProperty.Register(
                "ForwardInTransition",
                typeof(ViewTransition),
                typeof(ViewTransitions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the ForwardInAnimation property. This dependency property 
        /// indicates the animation to use during forward navigation to bring the new page into view.
        /// </summary>
        public ViewTransition ForwardInTransition
        {
            get { return (ViewTransition)GetValue(ForwardInTransitionProperty); }
            set { SetValue(ForwardInTransitionProperty, value); }
        }
        #endregion

        #region BackwardOutTransition

        /// <summary>
        /// BackwardOutTransition Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackwardOutTransitionProperty =
            DependencyProperty.Register(
                "BackwardOutTransition",
                typeof(ViewTransition),
                typeof(ViewTransitions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the BackwardOutTransition property. This dependency property 
        /// indicates the animation to use during backward navigation to remove the previous page from view.
        /// </summary>
        public ViewTransition BackwardOutTransition
        {
            get { return (ViewTransition)GetValue(BackwardOutTransitionProperty); }
            set { SetValue(BackwardOutTransitionProperty, value); }
        }

        #endregion

        #region BackwardInTransition

        /// <summary>
        /// BackwardInAnimation Dependency Property
        /// </summary>
        public static readonly DependencyProperty BackwardInTransitionProperty =
            DependencyProperty.Register(
                "BackwardInTransition",
                typeof(ViewTransition),
                typeof(ViewTransitions),
                new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the BackwardInAnimation property. This dependency property 
        /// indicates the animation to use during backward navigation to bring the new page into view.
        /// </summary>
        public ViewTransition BackwardInTransition
        {
            get { return (ViewTransition)GetValue(BackwardInTransitionProperty); }
            set { SetValue(BackwardInTransitionProperty, value); }
        }

        #endregion

        #region Mode (Dependency Property)

        /// <summary>
        /// Using a DependencyProperty as the backing store for  ModeProperty.  This enables animation, styling, binding, etc...    
        /// </summary>
        public static readonly DependencyProperty ModeProperty =
            DependencyProperty.Register(
                "Mode",
                typeof(ViewTransitionMode),
                typeof(ViewTransitions),
                new PropertyMetadata(ViewTransitionMode.Parallel)
            );

        /// <summary>
        /// The transitionmode for the page transitions.
        /// </summary>
        public ViewTransitionMode Mode
        {
            get { return (ViewTransitionMode)this.GetValue(ModeProperty); }
            set { this.SetValue(ModeProperty, value); }
        }

        #endregion
    }
}
