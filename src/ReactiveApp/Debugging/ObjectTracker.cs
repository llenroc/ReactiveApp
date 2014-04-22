using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveApp.Debugging
{
    /// <summary>
    /// Based on https://github.com/timheuer/callisto/blob/master/src/Callisto.TestApp/ObjectTracker.cs
    /// </summary>
    public class ObjectTracker : IObjectTracker
    {
        private static readonly object monitor = new object();
        private static readonly List<WeakReference> objects = new List<WeakReference>();
        private static bool? shouldTrack;
        
        public void TrackObject(object objectToTrack)
        {
            if (ShouldTrack())
            {
                lock (monitor)
                {
                    objects.Add(new WeakReference(objectToTrack));
                }
            }
        }

        private bool ShouldTrack()
        {
            if (shouldTrack == null)
            {
                shouldTrack = Debugger.IsAttached;
            }

            return shouldTrack.Value;
        }

        public IEnumerable<object> GetLiveTrackedObjects()
        {
            lock (monitor)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                return objects.Where(o => o.IsAlive).Select(o => o.Target);
            }
        }
        
        public void GarbageCollect()
        {
            // Garbage Collect
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var liveObjects = this.GetLiveTrackedObjects();
            
            StringBuilder sbStatus = new StringBuilder();
            
            Debug.WriteLine("---------------------------------------------------------------------");
            if (liveObjects.Count() == 0)
            {
                sbStatus.AppendLine("No Memory Leaks.");
            }
            else
            {
                sbStatus.AppendLine("***    Possible memory leaks in the objects below or their children.   ***");
                sbStatus.AppendLine("*** Clear memory again and see if any of the objects free from memory. ***");
            }
            foreach (object obj in liveObjects)
            {
                string strAliveObj = obj.GetType().ToString();
                sbStatus.AppendLine(strAliveObj);
            }
            sbStatus.AppendLine("----");
            //long lBytes = GC.GetTotalMemory(true);
            //sbStatus.AppendLine(string.Format("GC.GetTotalMemory(true): {0} Bytes, {1} MB", lBytes.ToString(), (lBytes / 1024 / 1024).ToString()));
            Debug.WriteLine(sbStatus.ToString());
            Debug.WriteLine("---------------------------------------------------------------------");
        }
    }

}
