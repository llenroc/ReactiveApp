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
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneReactiveView"/> class.
        /// </summary>
        public WinRTReactiveView()
        {
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(WinRTReactiveView), new PropertyMetadata(null));
        
        object IViewFor.ViewModel
        {
            get { return this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ReactiveViewModelRequest req = e.Parameter as ReactiveViewModelRequest;
            if (req == null)
            {
                throw new InvalidOperationException("Parameter should be of type ReactiveViewModelRequest");
            }

            this.ViewCreated(req);

            this.LoadState(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            this.SaveState(e);
        }

        protected virtual void LoadState(NavigationEventArgs e)
        {
        }

        protected virtual void SaveState(NavigationEventArgs e)
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
