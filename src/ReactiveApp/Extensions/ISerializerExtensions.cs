﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Services;

namespace ReactiveApp
{
    public static class ISerializerExtensions
    {
        public static T DeserializeObject<T>(this ISerializer This, string stringToSerialize)
        {
            return (T)This.DeserializeObject(typeof(T), stringToSerialize);
        }
    }
}
