using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using ReactiveUI;

namespace ReactiveApp.Xaml.Views
{
    public abstract class PhoneReactiveView : PhoneApplicationPage, IReactiveView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhoneReactiveView"/> class.
        /// </summary>
        public PhoneReactiveView()
        {
        }

        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(PhoneReactiveView), new PropertyMetadata(null));

        object IViewFor.ViewModel
        {
            get { return this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            ReactiveViewModelRequest req = this.ViewCreated(e.Uri);

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
}
