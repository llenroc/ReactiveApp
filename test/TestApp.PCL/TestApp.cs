using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp;
using ReactiveApp.App;
using TestApp.ViewModels;

namespace TestApp
{
    public class TestApp : ReactiveApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            //Register

            this.RegisterStartup<MainViewModel>();
        }
    }
}
