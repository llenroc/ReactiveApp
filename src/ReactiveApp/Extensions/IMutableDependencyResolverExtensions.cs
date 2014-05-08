using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ReactiveUI;
using Splat;

namespace ReactiveApp
{
    public static class IMutableDependencyResolverExtensions
    {
        public static void RegisterConstant<TType>(this IMutableDependencyResolver This, TType value, string contract = null)
        {
            This.Register(() => value, typeof(TType), contract);
        }

        public static void RegisterLazySingleton<TType>(this IMutableDependencyResolver This, Func<TType> valueFactory, string contract = null)
        {
            var val = new Lazy<TType>(valueFactory, LazyThreadSafetyMode.ExecutionAndPublication);
            This.Register(() => val.Value, typeof(TType), contract);
        }

        public static void RegisterLazySingleton<TType>(this IMutableDependencyResolver This, Func<IDependencyResolver, TType> valueFactory, string contract = null)
        {
            var val = new Lazy<TType>(() => valueFactory(This), LazyThreadSafetyMode.ExecutionAndPublication);
            This.Register(() => val.Value, typeof(TType), contract);
        }

        public static void Register<TType>(this IMutableDependencyResolver This, Func<TType> valueFactory, string contract = null)
        {
            This.Register(() => valueFactory(), typeof(TType), contract);
        }

        public static void Register<TType>(this IMutableDependencyResolver This, Func<IDependencyResolver, TType> valueFactory, string contract = null)
        {
            This.Register(() => valueFactory(This), typeof(TType), contract);
        }

        public static void RegisterView(this IMutableDependencyResolver This, Type viewModelType, Type viewType, string contract = null)
        {
            Type viewForType = typeof(IViewFor<>).MakeGenericType(viewModelType);
            if (!viewForType.GetTypeInfo().IsAssignableFrom(viewType.GetTypeInfo()))
            {
                throw new ArgumentException("ViewType is not assignable to IViewFor<ViewModelType>");
            }
            This.Register(() => viewType, typeof(IViewFor<>).MakeGenericType(viewModelType), contract);
        }

        public static void RegisterView<TView, TViewModel>(this IMutableDependencyResolver This, string contract = null) where TView : IViewFor<TViewModel> where TViewModel : class
        {
            Type viewType = typeof(TView);
            Type viewForType = typeof(IViewFor<TViewModel>);
            This.Register(() => viewType, viewForType, contract);
        }
    }
}
