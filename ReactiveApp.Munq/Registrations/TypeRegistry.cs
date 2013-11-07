// --------------------------------------------------------------------------------------------------
// © Copyright 2011 by Matthew Dennis.
// Released under the Microsoft Public License (Ms-PL) http://www.opensource.org/licenses/ms-pl.html
// --------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Munq
{
    internal class TypeRegistry : IDisposable
    {
        // Track whether Dispose has been called.
        private bool disposed;
        private readonly IDictionary<IRegistrationKey, List<Registration>> typeRegistrations = new Dictionary<IRegistrationKey, List<Registration>>();

        public void Add(Registration reg)
        {
            IRegistrationKey key = MakeKey(reg.Name, reg.ResolvesTo);

            List<Registration> registrationList;
            if (typeRegistrations.TryGetValue(key, out registrationList))
            {
                registrationList.Add(reg);
            }
            else
            {
                registrationList = new List<Registration>(2) { reg };
                typeRegistrations[key] = registrationList;
            }
        }

        public IEnumerable<Registration> Get(string name, Type type)
        {
            IRegistrationKey key = MakeKey(name, type);
            return typeRegistrations[key];
        }

        public IEnumerable<Registration> GetDerived(Type type)
        {
            var regs = typeRegistrations.Values
                        .SelectMany(list => list)
                        .Where(r => type.GetTypeInfo().IsAssignableFrom(r.ResolvesTo.GetTypeInfo()));
            return regs;
        }

        public IEnumerable<Registration> GetDerived(string name, Type type)
        {
            var regs = typeRegistrations.Values
                        .SelectMany(list => list)
                        .Where(r => string.Compare(r.Name, name, StringComparison.OrdinalIgnoreCase) == 0 &&
                                   type.GetTypeInfo().IsAssignableFrom(r.ResolvesTo.GetTypeInfo()));
            return regs;
        }

        public bool ContainsKey(string name, Type type)
        {
            IRegistrationKey key = MakeKey(name, type);
            return typeRegistrations.ContainsKey(key);
        }

        public IEnumerable<Registration> All(Type type)
        {
            return typeRegistrations.Values.SelectMany(list => list).Where(reg => reg.ResolvesTo == type);
        }

        public void Remove(IRegistration ireg)
        {
            IRegistrationKey key = MakeKey(ireg.Name, ireg.ResolvesTo);
            typeRegistrations.Remove(key);
            ireg.InvalidateInstanceCache();
        }

        private static IRegistrationKey MakeKey(string name, Type type)
        {
            return (name == null ? (IRegistrationKey)new UnNamedRegistrationKey(type)
                                 : (IRegistrationKey)new NamedRegistrationKey(name, type));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!disposed)
            {
                // If disposing equals true, dispose all ContainerLifetime instances
                if (disposing)
                {
                    foreach (Registration reg in typeRegistrations.Values.SelectMany(list => list))
                    {
                        var instance = reg.Instance as IDisposable;
                        if (instance != null)
                        {
                            instance.Dispose();
                            reg.Instance = null;
                        }
                        reg.InvalidateInstanceCache();
                    }
                }
            }
            disposed = true;
        }

        ~TypeRegistry()
        {
            Dispose(false);
        }
    }
}
