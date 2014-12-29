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
using WPNL.ViewModels;

namespace WPNL.UI.WP8.Views
{
    public partial class MainView : PhoneReactiveView, IViewFor<MainViewModel>
    {
        public MainView()
        {
            InitializeComponent();
        }

        public MainViewModel ViewModel
        {
            get { return (MainViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }
}