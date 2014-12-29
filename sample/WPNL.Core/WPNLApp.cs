using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ReactiveApp;
using ReactiveApp.App;
using Splat;
using WPNL.ViewModels;

namespace WPNL
{
    public class WPNLApp : ReactiveApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            //Register
            Locator.CurrentMutable.Register<MainViewModel>(c => new MainViewModel());

            this.RegisterStartup<MainViewModel>();
        }
    }
}
