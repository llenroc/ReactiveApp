using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using ReactiveApp.Xaml.Controls;

namespace ReactiveApp.Xaml.Services
{
    public class SystemTrayManager
    {
        private static IDisposable systemTrayDisposable;
        private static PhoneSystemTray systemTray;

        static SystemTrayManager()
        {
            systemTrayDisposable = Disposable.Empty;
            systemTray = new PhoneSystemTray();
        }

        internal static void Initialize(IPhoneFrameHelper f)
        {
            systemTrayDisposable.Dispose();
            systemTrayDisposable = Observable.FromEventPattern<NavigatedEventHandler, NavigationEventArgs>(h => f.Frame.Navigated += h, h => f.Frame.Navigated -= h)
                .Where(ep => ep.EventArgs.Content != null).Subscribe(ep =>
                {
                    PhoneApplicationPage page = ep.EventArgs.Content as PhoneApplicationPage;
                    if (page != null && page.GetType() == typeof(BackwardsCompatibilityPage))
                    {
                        systemTray.Initialize(page);
                    }
                    else
                    {
                        systemTray.Uninitialize();
                    }
                });
        }
    }
}
