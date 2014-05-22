using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ReactiveUI;

namespace ReactiveApp.Xaml.Views
{
    public class PhoneReactiveUserControl : UserControl, IViewFor
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(PhoneReactiveUserControl), new PropertyMetadata(null));
        
        object IViewFor.ViewModel
        {
            get { return this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }

    public class WinRTReactiveUserControl<T> : PhoneReactiveUserControl, IViewFor<T> where T : class
    {
        public T ViewModel
        {
            get { return (T)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }
}
