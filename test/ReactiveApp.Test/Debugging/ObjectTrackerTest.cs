using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveApp.Debugging;

namespace ReactiveApp.Test
{
    [TestClass]
    public class ObjectTrackerTest
    {
        private object trackObj;

        [TestInitialize]
        public void Setup()
        {
            this.trackObj = new object();
        }

        [TestMethod]
        public void TrackedObjectStillAliveTest()
        {
            IObjectTracker objectTracker = ObjectTracker.Instance;
            objectTracker.ForceTrack = true;

            objectTracker.TrackObject(trackObj);

            Assert.AreEqual(1, objectTracker.GetLiveTrackedObjects().Count());
        }

        [TestMethod]
        public void TrackedObjectDeadAfterNoRefTest()
        {
            IObjectTracker objectTracker = ObjectTracker.Instance;
            objectTracker.ForceTrack = true;

            objectTracker.TrackObject(trackObj);

            trackObj = null;

            Assert.AreEqual(0, objectTracker.GetLiveTrackedObjects().Count());
        }
    }
}
