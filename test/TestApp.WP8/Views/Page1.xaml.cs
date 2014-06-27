using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ReactiveApp.Xaml.Views;
using ReactiveUI;
using TestApp.ViewModels;

namespace TestApp.Views
{
    public partial class Page1 : PhoneReactiveView, IViewFor<TestViewModel>
    {
        public Page1()
        {
            InitializeComponent();
        }

        public TestViewModel ViewModel
        {
            get { return (TestViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }
}