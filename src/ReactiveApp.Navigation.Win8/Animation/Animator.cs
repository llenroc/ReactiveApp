using System;


#if !WINDOWS_PHONE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
#else
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
#endif

namespace ReactiveApp.Navigation
{
    public class DoubleAnimator
    {
        private readonly Storyboard storyboard = new Storyboard();
        private readonly DoubleAnimation animation = new DoubleAnimation();
        private Transform transform;
        private Action completionAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleAnimator"/> class.
        /// </summary>
        /// <param name="translateTransform">The translate transform.</param>
        /// <param name="property">The property.</param>
        public DoubleAnimator(Transform transform, string property)
        {
            this.transform = transform;
            this.storyboard.Completed += this.OnCompleted;
            this.storyboard.Children.Add(this.animation);
            Storyboard.SetTarget(this.animation, this.transform);
#if WINDOWS_PHONE
            Storyboard.SetTargetProperty(this.animation, new PropertyPath(property));
#else
            Storyboard.SetTargetProperty(this.animation, property);
#endif
        }

        public void GoTo(double targetOffset, Duration duration)
        {
            this.GoTo(targetOffset, duration, null, null);
        }

        public void GoTo(double targetOffset, Duration duration, Action completionAction)
        {
            this.GoTo(targetOffset, duration, null, completionAction);
        }

        public void GoTo(double targetOffset, Duration duration, EasingFunctionBase easingFunction)
        {
            this.GoTo(targetOffset, duration, easingFunction, null);
        }

        public void GoTo(double targetOffset, Duration duration, EasingFunctionBase easingFunction, Action completionAction)
        {
#if !WINDOWS_PHONE
            this.storyboard.SkipToFill();
#endif
            this.animation.To = targetOffset;
            this.animation.Duration = duration;
            this.animation.EasingFunction = easingFunction;
            this.storyboard.Begin();
            this.storyboard.SeekAlignedToLastTick(TimeSpan.Zero);
            this.completionAction = completionAction;
        }

        private void OnCompleted(object sender, object e)
        {
            Action action = this.completionAction;
            if (action != null && this.storyboard.GetCurrentState() != ClockState.Active)
            {
                this.completionAction = null;
                action();
            }
        }
    }
}
