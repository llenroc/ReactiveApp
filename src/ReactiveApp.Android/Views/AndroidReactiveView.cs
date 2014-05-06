using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using ReactiveApp.Activation;
using ReactiveApp.ViewModels;
using ReactiveUI;

namespace ReactiveApp.Android.Views
{
    public class AndroidReactiveView : Activity, IViewFor, IReactiveActivatable, IActivation
    {
        private ISubject<Tuple<IDataContainer, IDataContainer>> activated;
        private ISubject<Unit> deactivated;

        public AndroidReactiveView()
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

        protected override void OnResume()
        {
            base.OnResume();
        }
    }
}
