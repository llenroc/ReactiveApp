using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using ReactiveApp.ViewModels;
using ReactiveApp.Views;
using ReactiveUI;

namespace ReactiveApp.Android.Views
{
    public class AndroidReactiveView : ReactiveActivity, IReactiveView
    {
        private ReactiveViewModelRequest viewModelRequest;

        public AndroidReactiveView()
        {
        }

        object IViewFor.ViewModel
        {
            get;
            set;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            this.viewModelRequest = this.ViewCreated(this.Intent);
        }                                                                                                                                                                                                                   

        protected override void OnResume()
        {
            base.OnResume();

            this.LoadState();

        }

        protected override void OnPause()
        {
            base.OnPause();

            this.SaveState();
        }

        protected virtual void LoadState()
        {
        }

        protected virtual void SaveState()
        {
        }
    }
}
