using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ReactiveApp;
using ReactiveApp.Xaml.Views;
using ReactiveUI;
using TestApp.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace TestApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : WinRTReactiveView, IViewFor<MainViewModel>
    {
        public MainView()
        {
            this.InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x, x => x.HubSection1.DataContext));
                d(this.OneWayBind(this.ViewModel, x => x.FirstGroup, x => x.HubSection2.DataContext));
                d(this.OneWayBind(this.ViewModel, x => x.SecondGroup, x => x.HubSection3.DataContext));
                d(this.OneWayBind(this.ViewModel, x => x.ThirdGroup, x => x.HubSection4.DataContext));
                d(this.OneWayBind(this.ViewModel, x => x.FourthGroup, x => x.HubSection5.DataContext));
            });
        }

        public MainViewModel ViewModel
        {
            get { return (MainViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }
}
