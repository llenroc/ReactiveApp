using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Phone.Controls;

namespace ReactiveApp.Xaml.Adapters
{
    public interface IPhoneNavigationProvider : INavigationProvider
    {
        PhoneApplicationFrame Frame { get; }
    }
}
