using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Services
{
    public interface IOrientationManager
    {
        DisplayOrientations Orientation { get; }

        DisplayOrientations PreferredOrientation { get; set; }

        IObservable<DisplayOrientations> OrientationChanged { get; }
    }
}
