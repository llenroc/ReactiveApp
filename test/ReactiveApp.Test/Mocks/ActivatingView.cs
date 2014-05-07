using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Activation;
using ReactiveApp.ViewModels;
using ReactiveUI;

namespace ReactiveApp.Test
{
    public class ActivatingView : ReactiveObject, IViewFor<ActivatingViewModel>, IReactiveActivatable, IActivation
    {
        ActivatingViewModel viewModel;
        public ActivatingViewModel ViewModel
        {
            get { return viewModel; }
            set { this.RaiseAndSetIfChanged(ref viewModel, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (ActivatingViewModel)value; }
        }

        public ActivatingView()
        {
            this.WhenActivatedWithState((param, state, d) =>
            {
                IsActiveCount++;
                d(Disposable.Create(() => IsActiveCount--));
            });
        }

        public int IsActiveCount { get; set; }

        public Subject<Tuple<IDataContainer, IDataContainer>> ActivatedSubject = new Subject<Tuple<IDataContainer, IDataContainer>>();
        public IObservable<Tuple<IDataContainer, IDataContainer>> Activated
        {
            get { return this.ActivatedSubject; }
        }

        public Subject<Unit> DeactivatedSubject = new Subject<Unit>();
        public IObservable<Unit> Deactivated
        {
            get { return this.DeactivatedSubject; }
        }
    }
}
