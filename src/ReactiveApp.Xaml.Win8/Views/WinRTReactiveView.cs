using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using ReactiveUI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace ReactiveApp.Xaml.Views
{
    public class WinRTReactiveView : Page, IReactiveView
    {
        private ISubject<Tuple<IDataContainer, IDataContainer>> activated;
        private ISubject<Unit> deactivated;
        protected IDataContainer stateContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneReactiveView"/> class.
        /// </summary>
        public WinRTReactiveView()
        {
            this.Activated = this.activated = new Subject<Tuple<IDataContainer, IDataContainer>>();
            this.Deactivated = this.deactivated = new Subject<Unit>();
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(WinRTReactiveView), new PropertyMetadata(null));
        
        object IViewFor.ViewModel
        {
            get { return this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        public IObservable<Tuple<IDataContainer, IDataContainer>> Activated { get; private set; }

        public IObservable<Unit> Deactivated { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ReactiveViewModelRequest req = e.Parameter as ReactiveViewModelRequest;
            if (req == null)
            {
                throw new InvalidOperationException("Parameter should be of type ReactiveViewModelRequest");
            }

            this.ViewCreated(req);

            //load and activate
            this.stateContainer = this.LoadStateContainer(e);
            this.activated.OnNext(Tuple.Create(req.Parameters, this.stateContainer));
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            // deactivate and save
            this.deactivated.OnNext(Unit.Default);
            this.SaveStateContainer(e, this.stateContainer);
        }

        protected virtual IDataContainer LoadStateContainer(NavigationEventArgs e)
        {
            return new DataContainer();
        }

        protected virtual void SaveStateContainer(NavigationEventArgs e, IDataContainer state)
        {

        }
    }

    public class WinRTReactiveView<T> : WinRTReactiveView, IReactiveView<T> where T :class
    {        
        public T ViewModel
        {
            get { return (T)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }
}
