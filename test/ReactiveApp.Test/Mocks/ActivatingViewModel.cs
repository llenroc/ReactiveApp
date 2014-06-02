using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.ViewModels;
using ReactiveUI;

namespace ReactiveApp.Test
{
    public class ActivatingViewModel : ReactiveViewModel
    {
        public int IsActiveCount { get; protected set; }

        public ActivatingViewModel()
        {
            this.WhenActivated(d =>
            {
                IsActiveCount++;
                d(Disposable.Create(() => IsActiveCount--));
            });
        }
    }
}
