using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp;
using ReactiveApp.App;
using Splat;
using TestApp.Services;
using TestApp.ViewModels;

namespace TestApp
{
    public class TestApp : ReactiveApplication
    {
        public override void Initialize()
        {
            base.Initialize();

            //Register
            Locator.CurrentMutable.Register<ISampleDataService>(c => new SampleDataService());

            Locator.CurrentMutable.Register<MainViewModel>(c => new MainViewModel(c.GetService<ISampleDataService>()));

            this.RegisterStartup<MainViewModel>();
        }
    }
}
