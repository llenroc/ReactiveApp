using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveApp
{
    public static class ObservableExtensions
    {
        /// <summary>
        /// In RxUI6, ToProperty no longer schedules its notifications on the UI. 
        /// This function does.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="This">The this.</param>
        /// <param name="viewModel">The view model.</param>
        /// <param name="property">The property.</param>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static ObservableAsPropertyHelper<T> ToUIProperty<TViewModel,T>(this IObservable<T> This, TViewModel viewModel, Expression<Func<TViewModel, T>> property, out ObservableAsPropertyHelper<T> result, T initialValue = default(T))
            where TViewModel : ReactiveObject
        {
            return This.ToProperty(viewModel, property, out result, initialValue, RxApp.MainThreadScheduler);
        }
    }
}
