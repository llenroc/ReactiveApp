using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ReactiveApp.Xaml.Views;
using ReactiveUI;
using TestApp.Models;
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

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TestApp.UserControls
{
    public sealed partial class TitleItemTemplate : WinRTReactiveUserControl, IViewFor<ItemViewModel>
    {
        public TitleItemTemplate()
        {
            this.InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Title, x => x.Title.Text));
                d(this.OneWayBind(this.ViewModel, x => x.Subtitle, x => x.Subtitle.Text));
            });
        }

        public ItemViewModel ViewModel
        {
            get { return (ItemViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }
    }
}
