using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace ReactiveApp.Test
{
    public class DerivedActivatingViewModel : ActivatingViewModel
    {
        public int IsActiveCountAlso { get; protected set; }

        public DerivedActivatingViewModel()
        {
            this.WhenActivated(d =>
            {
                IsActiveCountAlso++;
                d(Disposable.Create(() => IsActiveCountAlso--));
            });
        }
    }
}
