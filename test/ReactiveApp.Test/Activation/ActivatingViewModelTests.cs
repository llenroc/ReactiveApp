using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveApp.Activation;
using ReactiveApp.ViewModels;
using ReactiveUI;

namespace ReactiveApp.Test
{
    [TestClass]
    public class ActivatingViewModelTests
    {
        [TestMethod]
        public void ActivationsGetRefCounted()
        {
            var vm = new ActivatingViewModel();
            Assert.AreEqual(0, vm.IsActiveCount);

            vm.ReactiveActivator.Activate(new DataContainer(), new DataContainer());
            Assert.AreEqual(1, vm.IsActiveCount);

            vm.ReactiveActivator.Activate(new DataContainer(), new DataContainer());
            Assert.AreEqual(1, vm.IsActiveCount);

            vm.ReactiveActivator.Deactivate();
            Assert.AreEqual(1, vm.IsActiveCount);

            // Refcount drops to zero
            vm.ReactiveActivator.Deactivate();
            Assert.AreEqual(0, vm.IsActiveCount);
        }

        [TestMethod]
        public void DerivedActivationsDontGetStomped()
        {
            var vm = new DerivedActivatingViewModel();
            Assert.AreEqual(0, vm.IsActiveCount);
            Assert.AreEqual(0, vm.IsActiveCountAlso);

            vm.ReactiveActivator.Activate(new DataContainer(), new DataContainer());
            Assert.AreEqual(1, vm.IsActiveCount);
            Assert.AreEqual(1, vm.IsActiveCountAlso);

            vm.ReactiveActivator.Activate(new DataContainer(), new DataContainer());
            Assert.AreEqual(1, vm.IsActiveCount);
            Assert.AreEqual(1, vm.IsActiveCountAlso);

            vm.ReactiveActivator.Deactivate();
            vm.ReactiveActivator.Deactivate();
            Assert.AreEqual(0, vm.IsActiveCount);
            Assert.AreEqual(0, vm.IsActiveCountAlso);
        }
    }    
}
