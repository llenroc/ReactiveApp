using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp;
using ReactiveApp.App;
using WPNL.Core.ViewModels;

namespace WPNL.Core
{
    public class WPNLApp : ReactiveApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            //Register

            this.RegisterStartup<MainViewModel>();
        }
    }
}
