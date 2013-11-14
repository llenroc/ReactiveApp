using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveApp.Interfaces;

#if WINDOWS_PHONE
using System.Windows.Controls;
#else
using Windows.UI.Xaml.Controls;
#endif

namespace ReactiveApp.Xaml.Controls
{
    public class ReactiveShell : ContentControl, IShell<ReactiveShell,ReactiveView>
    {
        public Task BackAsync()
        {
            throw new NotImplementedException();
        }

        public IObservable<bool> CanBack
        {
            get { throw new NotImplementedException(); }
        }

        public Task ViewAsync<V>(V view = default(V)) where V : ReactiveView
        {
            throw new NotImplementedException();
        }

        public IObservable<bool> CanView
        {
            get { throw new NotImplementedException(); }
        }

        public Task ForwardAsync()
        {
            throw new NotImplementedException();
        }

        public IObservable<bool> CanForward
        {
            get { throw new NotImplementedException(); }
        }

        public Stack<ReactiveView> BackStack
        {
            get { throw new NotImplementedException(); }
        }

        public Stack<ReactiveView> ForwardStack
        {
            get { throw new NotImplementedException(); }
        }

        public IObservable<NavigatingInfo> Navigating
        {
            get { throw new NotImplementedException(); }
        }

        public IObservable<NavigatedInfo> Navigated
        {
            get { throw new NotImplementedException(); }
        }

        public IObservable<IJournalEntry> ViewJournal
        {
            get { throw new NotImplementedException(); }
        }

        public Task Activate()
        {
            throw new NotImplementedException();
        }
    }
}
