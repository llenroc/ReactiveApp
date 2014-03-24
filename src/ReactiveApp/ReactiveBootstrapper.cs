using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;

namespace ReactiveApp
{
    public abstract class ReactiveBootstrapper
    {
        protected abstract IReactiveApplication CreateApplication();

        public virtual void Run()
        {

        }
    }
}
