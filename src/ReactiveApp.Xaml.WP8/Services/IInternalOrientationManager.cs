using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;
using ReactiveApp.Services;

namespace ReactiveApp.Xaml.Services
{
    internal interface IInternalOrientationManager : IOrientationManager
    {
        void Initialize(PhoneApplicationFrame frame);
    }
}
