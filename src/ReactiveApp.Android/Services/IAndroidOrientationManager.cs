using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using ReactiveApp.Services;

namespace ReactiveApp.Android.Services
{
    internal interface IAndroidOrientationManager : IOrientationManager
    {
        void Initialize(Application application);
    }
}
