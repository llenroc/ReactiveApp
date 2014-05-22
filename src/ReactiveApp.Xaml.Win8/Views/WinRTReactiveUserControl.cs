using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ReactiveApp.Xaml.Views
{
    public class WinRTReactiveUserControl : UserControl, IViewFor
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(object), typeof(WinRTReactiveUserControl), new PropertyMetadata(null));
        
        object IViewFor.ViewModel
        {
            get { return this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }

    public class WinRTReactiveUserControl<T> : WinRTReactiveUserControl, IViewFor<T> where T : class
    {
        public T ViewModel
        {
            get { return (T)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }
}
