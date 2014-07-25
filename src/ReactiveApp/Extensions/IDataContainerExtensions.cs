using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;
using ReactiveApp.ViewModels;
using Splat;

namespace ReactiveApp
{
    public static class IDataContainerExtensions
    {
        public static void Write(this IDataContainer This, string key, object toStore)
        {
            if (toStore == null)
            {
                return;
            }

            ISerializer serializer = Locator.Current.GetService<ISerializer>();
            string serialized = serializer.SerializeObject(toStore);

            This.Data[key] = serialized;
        }

        public static T Read<T>(this IDataContainer This, string key)
            where T : new()
        {
            return (T)This.Read(key, typeof(T));
        }

        public static object Read(this IDataContainer This, string key, Type type)
        {
            ISerializer serializer = Locator.Current.GetService<ISerializer>();
            return serializer.DeserializeObject(type, This.Data[key]);
        }
    }
}
