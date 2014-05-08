using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using ReactiveApp.App;
using ReactiveApp.Services;
using ReactiveApp.Xaml;
using ReactiveApp.Xaml.Adapters;
using WPNL.Core;

namespace WPNL.UI.WP8
{
    public class Bootstrapper : PhoneBootstrapper
    {
        public Bootstrapper(PhoneApplicationFrame frame, IArgumentsProvider arguments)
            :base(frame, arguments)
        {

        }

        protected override IReactiveApplication CreateApplication()
        {
            return new WPNLApp();
        }

        protected override INavigationSerializer CreateNavigationSerializer()
        {
            return base.CreateNavigationSerializer();
        }
    }
}
