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
using Splat;

namespace ReactiveApp.Test
{
    [TestClass]
    public class ActivatingViewTests
    {
        private ModernDependencyResolver locator;

        [TestInitialize]
        public void Setup()
        {
            locator = new ModernDependencyResolver();
            locator.InitializeSplat();
            locator.InitializeReactiveUI();
            locator.Register(() => new ActivationViewFetcher(), typeof(IReactiveActivationForViewFetcher));
        }

        [TestMethod]
        public void ActivatingViewSmokeTest()
        {
            using (locator.WithResolver())
            {
                var vm = new ActivatingViewModel();
                var fixture = new ActivatingView();

                fixture.ViewModel = vm;
                Assert.AreEqual(0, vm.IsActiveCount);
                Assert.AreEqual(0, fixture.IsActiveCount);

                fixture.ActivatedSubject.OnNext(Tuple.Create<IDataContainer, IDataContainer>(new DataContainer(), new DataContainer()));
                Assert.AreEqual(1, vm.IsActiveCount);
                Assert.AreEqual(1, fixture.IsActiveCount);

                fixture.DeactivatedSubject.OnNext(Unit.Default);
                Assert.AreEqual(0, vm.IsActiveCount);
                Assert.AreEqual(0, fixture.IsActiveCount);
            }
        }

        [TestMethod]
        public void NullingViewModelShouldDeactivateIt()
        {
            using (locator.WithResolver())
            {
                var vm = new ActivatingViewModel();
                var fixture = new ActivatingView();

                fixture.ViewModel = vm;
                Assert.AreEqual(0, vm.IsActiveCount);
                Assert.AreEqual(0, fixture.IsActiveCount);

                fixture.ActivatedSubject.OnNext(Tuple.Create<IDataContainer, IDataContainer>(new DataContainer(), new DataContainer()));
                Assert.AreEqual(1, vm.IsActiveCount);
                Assert.AreEqual(1, fixture.IsActiveCount);

                fixture.ViewModel = null;
                Assert.AreEqual(0, vm.IsActiveCount);
            }
        }

        [TestMethod]
        public void SwitchingViewModelShouldDeactivateIt()
        {
            using (locator.WithResolver())
            {
                var vm = new ActivatingViewModel();
                var fixture = new ActivatingView();

                fixture.ViewModel = vm;
                Assert.AreEqual(0, vm.IsActiveCount);
                Assert.AreEqual(0, fixture.IsActiveCount);

                fixture.ActivatedSubject.OnNext(Tuple.Create<IDataContainer, IDataContainer>(new DataContainer(), new DataContainer()));
                Assert.AreEqual(1, vm.IsActiveCount);
                Assert.AreEqual(1, fixture.IsActiveCount);

                var newVm = new ActivatingViewModel();
                Assert.AreEqual(0, newVm.IsActiveCount);

                fixture.ViewModel = newVm;
                Assert.AreEqual(0, vm.IsActiveCount);
                Assert.AreEqual(1, newVm.IsActiveCount);
            }
        }

        [TestMethod]
        public void SettingViewModelAfterLoadedShouldLoadIt()
        {
            using (locator.WithResolver())
            {
                var vm = new ActivatingViewModel();
                var fixture = new ActivatingView();

                Assert.AreEqual(0, vm.IsActiveCount);
                Assert.AreEqual(0, fixture.IsActiveCount);

                fixture.ActivatedSubject.OnNext(Tuple.Create<IDataContainer, IDataContainer>(new DataContainer(), new DataContainer()));
                Assert.AreEqual(1, fixture.IsActiveCount);

                fixture.ViewModel = vm;
                Assert.AreEqual(1, fixture.IsActiveCount);
                Assert.AreEqual(1, vm.IsActiveCount);

                fixture.DeactivatedSubject.OnNext(Unit.Default);
                Assert.AreEqual(0, fixture.IsActiveCount);
                Assert.AreEqual(0, vm.IsActiveCount);
            }
        }
    }
}
