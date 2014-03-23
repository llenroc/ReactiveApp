using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

#if !WINDOWS_PHONE
using Windows.Foundation;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
#else
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
#endif

namespace ReactiveApp.Xaml.Services
{
    public class GestureService
    {
        private DragLock dragLock;
        private bool dragging;
        private WeakReference gestureSource;
        private Point gestureOrigin;

#if !WINDOWS_PHONE
        private IObservable<EventPattern<ManipulationDeltaRoutedEventArgs>> drag;
#else
        private IObservable<EventPattern<ManipulationDeltaEventArgs>> drag;
#endif
        
        public IObservable<GestureStart> GestureStart
        {
            get;
            private set;
        }

        public IObservable<GestureEnd> GestureEnd
        {
            get;
            private set;
        }

        public IObservable<GestureDrag> HorizontalDrag
        {
            get;
            private set;
        }

        public IObservable<GestureDrag> VerticalDrag
        {
            get;
            private set;
        }

        public Size DeadZoneInPixels { get; set; }

        protected UIElement Target { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GestureService" /> class.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="shouldHandleAllDrags">if set to <c>true</c> [should handle all drags].</param>
        public GestureService(UIElement target)
        {
            this.Target = target;
            this.DeadZoneInPixels = Size.Empty;
                        
#if !WINDOWS_PHONE
            this.GestureStart = 
                Observable.FromEventPattern<ManipulationStartedEventHandler, ManipulationStartedRoutedEventArgs>(h => this.Target.ManipulationStarted += h, h => this.Target.ManipulationStarted -= h)
                    .Select(ep => this.StartMove(ep.EventArgs.Container, ep.EventArgs.Position))
                    .Publish().PermaRef();
            this.GestureEnd = 
                Observable.FromEventPattern<ManipulationCompletedEventHandler, ManipulationCompletedRoutedEventArgs>(h => this.Target.ManipulationCompleted += h, h => this.Target.ManipulationCompleted -= h)
                    .Select(ep => this.EndMove(ep.EventArgs.IsInertial, ep.EventArgs.Cumulative, ep.EventArgs.Velocities, ep.EventArgs.Position))
                    .Publish().PermaRef();
            this.drag = Observable.FromEventPattern<ManipulationDeltaEventHandler, ManipulationDeltaRoutedEventArgs>(h => this.Target.ManipulationDelta += h, h => this.Target.ManipulationDelta -= h)
                .Where(ep => Math.Abs(ep.EventArgs.Cumulative.Translation.X) > this.DeadZoneInPixels.Width || Math.Abs(ep.EventArgs.Cumulative.Translation.Y) > this.DeadZoneInPixels.Height)
                .Publish().RefCount();
            this.HorizontalDrag = this.drag.Where(ep => this.GetDragLockForMove(ep.EventArgs.Cumulative) == DragLock.Horizontal && ep.EventArgs.Delta.Translation.X != 0.0)
                .Select(ep => new GestureDrag(ep.EventArgs.Cumulative.Translation, ep.EventArgs.Delta.Translation, ep.EventArgs.Position));
            this.VerticalDrag = this.drag.Where(ep => this.GetDragLockForMove(ep.EventArgs.Cumulative) == DragLock.Vertical && ep.EventArgs.Delta.Translation.Y != 0.0)
                .Select(ep => new GestureDrag(ep.EventArgs.Cumulative.Translation, ep.EventArgs.Delta.Translation, ep.EventArgs.Position));
#else        
            this.GestureStart = 
                Observable.FromEventPattern<ManipulationStartedEventArgs>(h => this.Target.ManipulationStarted += h, h => this.Target.ManipulationStarted -= h)
                    .Select(ep => this.StartMove(ep.EventArgs.ManipulationContainer, ep.EventArgs.ManipulationOrigin))
                    .Publish().PermaRef();
            this.GestureEnd =   
                Observable.FromEventPattern<ManipulationCompletedEventArgs>(h => this.Target.ManipulationCompleted += h, h => this.Target.ManipulationCompleted -= h)
                    .Select(ep => this.EndMove(ep.EventArgs.IsInertial, ep.EventArgs.TotalManipulation, ep.EventArgs.FinalVelocities, ep.EventArgs.ManipulationOrigin))
                    .Publish().PermaRef();
            this.drag = Observable.FromEventPattern<ManipulationDeltaEventArgs>(h => this.Target.ManipulationDelta += h, h => this.Target.ManipulationDelta -= h)
                .Where(ep => Math.Abs(ep.EventArgs.CumulativeManipulation.Translation.X) > this.DeadZoneInPixels.Width || Math.Abs(ep.EventArgs.CumulativeManipulation.Translation.Y) > this.DeadZoneInPixels.Height)
                .Publish().RefCount();
            this.HorizontalDrag = this.drag.Where(ep => this.GetDragLockForMove(ep.EventArgs.CumulativeManipulation) == DragLock.Horizontal && ep.EventArgs.DeltaManipulation.Translation.X != 0.0)
                .Select(ep => new GestureDrag(ep.EventArgs.CumulativeManipulation.Translation, ep.EventArgs.DeltaManipulation.Translation, ep.EventArgs.ManipulationOrigin));
            this.VerticalDrag = this.drag.Where(ep => this.GetDragLockForMove(ep.EventArgs.CumulativeManipulation) == DragLock.Vertical && ep.EventArgs.DeltaManipulation.Translation.Y != 0.0)
                .Select(ep => new GestureDrag(ep.EventArgs.CumulativeManipulation.Translation, ep.EventArgs.DeltaManipulation.Translation, ep.EventArgs.ManipulationOrigin));
#endif
        }

        protected virtual GestureStart StartMove(object container, Point position)
        {
            this.gestureSource = new WeakReference(container);
            this.gestureOrigin = position;
            this.dragLock = DragLock.Unset;
            this.dragging = false;
            return new GestureStart(position);
        }

        protected virtual GestureEnd EndMove(bool isInertial, ManipulationDelta cumulative, ManipulationVelocities velocities, Point position)
        {
            this.dragLock = DragLock.Unset;
            this.dragging = false;
            double angle = 0.0;
            if (isInertial)
            {
                angle = GestureService.AngleFromVector(velocities.LinearVelocity.X, velocities.LinearVelocity.Y);
                if (angle <= 45.0 || angle >= 315.0)
                {
                    angle = 0.0;
                }
                else if (angle >= 135.0 && angle <= 225.0)
                {
                    angle = 180.0;
                }
                this.ReleaseMouseCaptureAtGestureOrigin();
            }
            return new GestureEnd(angle, isInertial, cumulative.Translation, position);
        }

        private DragLock GetDragLockForMove(ManipulationDelta cumulative)
        {
            if (!this.dragging)
            {
                this.ReleaseMouseCaptureAtGestureOrigin();
            }
            this.dragging = true;
            if (this.dragLock == DragLock.Unset)
            {
                double num = GestureService.AngleFromVector(cumulative.Translation.X, cumulative.Translation.Y) % 180.0;
                this.dragLock = num <= 45.0 || num >= 135.0 ? DragLock.Horizontal : DragLock.Vertical;
            }
            return this.dragLock;
        }

        private void ReleaseMouseCaptureAtGestureOrigin()
        {
            if (this.gestureSource == null)
            {
                return;
            }
            FrameworkElement frameworkElement = this.gestureSource.Target as FrameworkElement;
            if (frameworkElement == null)
            {
                return;
            }
            foreach (UIElement uiElement in VisualTreeHelper.FindElementsInHostCoordinates(
#if WINDOWS_PHONE
                frameworkElement.TransformToVisual(null).Transform(this.gestureOrigin), Application.Current.RootVisual))
            {
                uiElement.ReleaseMouseCapture();
            }
#else
                frameworkElement.TransformToVisual(null).TransformPoint(this.gestureOrigin), Window.Current.Content))
            {
                uiElement.ReleasePointerCaptures();
            }
#endif
        }

        private static double AngleFromVector(double x, double y)
        {
            double angle = Math.Atan2(y, x);
            if (angle < 0.0)
            {
                angle = 2.0 * Math.PI + angle;
            }
            //convert to degrees
            return angle * (180.0 / Math.PI);
        }

        private enum DragLock
        {
            Unset,
            Free,
            Vertical,
            Horizontal,
        }
    }

    public class GestureStart
    {
        public Point Position { get; private set; }

        internal GestureStart(Point position)
        {
            this.Position = position;
        }
    }

    public class GestureEnd
    {
        public double Angle { get; private set; }

        public bool IsFlick { get; private set; }

        public Point CumulativeDistance { get; private set; }

        public Point Position { get; private set; }

        internal GestureEnd(double angle, bool isFlick, Point cumulativeDistance, Point position)
        {
            this.Angle = angle;
            this.IsFlick = isFlick;
            this.CumulativeDistance = cumulativeDistance;
            this.Position = position;
        }
    }

    public class GestureDrag
    {
        public Point DeltaDistance { get; private set; }

        public Point CumulativeDistance { get; private set; }

        public Point Position { get; private set; }

        internal GestureDrag(Point cumulativeTranslation, Point deltaTranslation, Point position)
        {
            this.CumulativeDistance = cumulativeTranslation;
            this.DeltaDistance = deltaTranslation;
            this.Position = position;
        }
    }
}