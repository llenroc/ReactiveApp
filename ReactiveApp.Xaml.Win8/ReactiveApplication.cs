using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !WINDOWS_PHONE
using Windows.UI.Xaml;
#else
using System.Windows;
#endif

namespace ReactiveApp.Xaml
{
    public class ReactiveApplication : Application
    {
        public ReactiveApplication()
        {

        }

        protected virtual void Configure()
        {

        }
    }
}
