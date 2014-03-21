using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !WINDOWS_PHONE
using Windows.UI;
using Windows.UI.Xaml;
using ReactiveApp.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Media;
#endif

namespace ReactiveApp.Xaml.Services
{
    interface ISystemTrayManager
    {
        Color Foreground { get; set; }

        Color Background { get; set; }

        double Opacity { get; set; }

        bool IsVisible { get; set; }

        IProgressIndicator ProgressIndicator { get; }
    }
}
