using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using ReactiveUI;

namespace TestApp
{
    public class ItemViewModel : ReactiveObject
    {
        private string lineOne;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineOne
        {
            get { return this.lineOne; }
            set { this.RaiseAndSetIfChanged(ref this.lineOne, value); }
        }

        private string lineTwo;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineTwo
        {
            get { return this.lineTwo; }
            set { this.RaiseAndSetIfChanged(ref this.lineTwo, value); }
        }

        private string lineThree;
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding.
        /// </summary>
        /// <returns></returns>
        public string LineThree
        {
            get { return this.lineThree; }
            set { this.RaiseAndSetIfChanged(ref this.lineThree, value); }
        }
    }
}