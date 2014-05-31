using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using ReactiveUI;

namespace ReactiveApp.iOS.Views
{
    public class iOSReactiveView : UIViewController, IReactiveView
    {
        private ISubject<Tuple<IDataContainer, IDataContainer>> activated;
        private ISubject<Unit> deactivated;
        protected IDataContainer stateContainer;

        public iOSReactiveView()
        {
            this.activated = new Subject<Tuple<IDataContainer, IDataContainer>>();
            this.deactivated = new Subject<Unit>();
        }

        object IViewFor.ViewModel
        {
            get;
            set;
        }

        public ReactiveViewModelRequest Request { get; set; }

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

            this.ViewCreated(this.Request);

            this.activated.OnNext(Tuple.Create(this.Request.Parameters, this.stateContainer));
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            this.deactivated.OnNext(Unit.Default);
        }
    }
}
