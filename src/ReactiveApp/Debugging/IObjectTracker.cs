﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Debugging
{
    public interface IObjectTracker
    {
        void TrackObject(object objectToTrack);

        IEnumerable<object> GetLiveTrackedObjects();

        void GarbageCollect();
    }
}
