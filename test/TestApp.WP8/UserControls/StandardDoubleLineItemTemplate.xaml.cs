using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ReactiveApp.Xaml.Views;
using ReactiveUI;
using TestApp.Models;
using TestApp.ViewModels;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace TestApp.UserControls
{
    public sealed partial class StandardDoubleLineItemTemplate : PhoneReactiveUserControl, IViewFor<ItemViewModel>
    {
        public StandardDoubleLineItemTemplate()
        {
            this.InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(this.ViewModel, x => x.Image, x => x.Image.Source));
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
