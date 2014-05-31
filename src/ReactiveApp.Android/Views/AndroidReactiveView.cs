using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using ReactiveUI;

namespace ReactiveApp.Android.Views
{
    public class AndroidReactiveView : Activity, IReactiveView
    {
        private ISubject<Tuple<IDataContainer, IDataContainer>> activated;
        private ISubject<Unit> deactivated;
        protected IDataContainer stateContainer;
        private ReactiveViewModelRequest viewModelRequest;

        public AndroidReactiveView()
        {
            this.activated = new Subject<Tuple<IDataContainer, IDataContainer>>();
            this.deactivated = new Subject<Unit>();
        }

        object IViewFor.ViewModel
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.viewModelRequest = this.ViewCreated(this.Intent);

            // load state so we can use it when onResume is called
            this.stateContainer = this.LoadStateContainer(savedInstanceState);
        }                                                                                                                                                                                                                   

        protected override void OnResume()
        {
            base.OnResume();

            this.activated.OnNext(Tuple.Create(this.viewModelRequest.Parameters, this.stateContainer));
        }

        protected override void OnPause()
        {
            base.OnPause();

            this.deactivated.OnNext(Unit.Default);
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);

            this.SaveStateContainer(outState, this.stateContainer);
        }

        protected virtual IDataContainer LoadStateContainer(Bundle bundle)
        {
            return new DataContainer();
        }

        protected virtual void SaveStateContainer(Bundle bundle, IDataContainer state)
        {
        }
    }
}
