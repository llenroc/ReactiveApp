using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reactive;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ReactiveApp.Services;
using ReactiveApp.Xaml.Views;
using ReactiveUI;
using TestApp.ViewModels;
using TestApp.Resources;
using ReactiveApp;

namespace TestApp
{
    public partial class MainView : PhoneReactiveView, IViewFor<MainViewModel>
    {
        // Constructor
        public MainView()
        {
            InitializeComponent();

            this.WhenActivatedWithState((param, state, d) =>
            {
                d(this.OneWayBind(this.ViewModel, x => x, x => x.PanoramaItem1.ItemsSource));
                d(this.OneWayBind(this.ViewModel, x => x.FirstGroup, x => x.PanoramaItem2.ItemsSource));
                d(this.OneWayBind(this.ViewModel, x => x.SecondGroup, x => x.PanoramaItem3.ItemsSource));
                d(this.OneWayBind(this.ViewModel, x => x.ThirdGroup, x => x.PanoramaItem4.ItemsSource));
                d(this.OneWayBind(this.ViewModel, x => x.FourthGroup, x => x.PanoramaItem5.ItemsSource));
            });
        }

        public MainViewModel ViewModel
        {
            get { return (MainViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }
}