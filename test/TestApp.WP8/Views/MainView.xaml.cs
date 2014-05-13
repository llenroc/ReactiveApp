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

namespace TestApp
{
    public partial class MainView : PhoneReactiveView, IViewFor<MainViewModel>
    {
        // Constructor
        public MainView()
        {
            InitializeComponent();
        }

        public MainViewModel ViewModel
        {
            get { return (MainViewModel)this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}