using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;
using Splat;

namespace ReactiveApp.App
{
    public class ReactiveApplication : IReactiveApplication
    {
        public virtual void Initialize()
        {
        }

        public void RegisterStartup(IStartup startup)
        {
            if(startup == null)
            {
                throw new ArgumentNullException("startup");
            }
            Locator.CurrentMutable.RegisterConstant(startup);
        }
    }
}
