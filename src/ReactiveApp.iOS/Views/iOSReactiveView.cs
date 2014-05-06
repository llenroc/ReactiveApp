using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using MonoTouch.UIKit;
using ReactiveApp.Activation;
using ReactiveApp.ViewModels;
using ReactiveUI;

namespace ReactiveApp.iOS.Views
{
    public class iOSReactiveView : UIViewController, IViewFor, IReactiveActivatable, IActivation
    {
        private ISubject<Tuple<IDataContainer, IDataContainer>> activated;
        private ISubject<Unit> deactivated;

        public iOSReactiveView()
        {
            this.activated = new Subject<Tuple<IDataContainer, IDataContainer>>();
            this.deactivated = new Subject<Unit>();
        }

        public object ViewModel
        {
            get;
            set;
        }

        public IObservable<Tuple<IDataContainer, IDataContainer>> Activated
        {
            get { return this.activated; }
        }

        public IObservable<Unit> Deactivated
        {
            get { return this.deactivated; }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
        }
    }
}
