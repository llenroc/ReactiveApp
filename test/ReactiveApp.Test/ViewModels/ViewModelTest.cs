using System;
using System.Reactive.Linq;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveApp.Platform;
using ReactiveApp.Test.Platform;
using ReactiveApp.ViewModels;

namespace ReactiveApp.Test.ViewModels
{
    [TestClass]
    public class ViewModelTest
    {
        private TestPlatformProvider provider;

        [TestInitialize]
        public void Setup()
        {
            provider = new TestPlatformProvider();
            provider.ViewEvents = new TestViewEvents(Observable.Return(new object()), Observable.Return(new object()));
        }

        [TestMethod]
        public void CacheViewTrueCachesView()
        {
            PlatformProvider.Instance = provider;

            ReactiveViewModel vm = new ReactiveViewModel();

            object view = new object();
            vm.AttachView(view);

            Assert.IsNotNull(vm.GetView());
        }

        [TestMethod]
        public void CacheViewFalseDoesNotCacheView()
        {
            PlatformProvider.Instance = provider;

            ReactiveViewModel vm = new ReactiveViewModel(false);

            object view = new object();
            vm.AttachView(view);

            Assert.IsNull(vm.GetView());
        }

        [TestMethod]
        public void CacheViewChangingDoesNotCacheView()
        {
            PlatformProvider.Instance = provider;

            TestViewModel vm = new TestViewModel();

            object view = new object();
            vm.AttachView(view);

            Assert.IsNotNull(vm.GetView());

            vm.DisableCacheView();

            Assert.IsNull(vm.GetView());
        }

        [TestMethod]
        public void ActivateDeactivateTest()
        {
            ReactiveViewModel vm = new ReactiveViewModel();

            Assert.IsFalse(vm.IsInitialized);
            Assert.IsFalse(vm.IsActive);

            vm.Activate();

            Assert.IsTrue(vm.IsInitialized);
            Assert.IsTrue(vm.IsActive);

            vm.Deactivate(true);

            Assert.IsTrue(vm.IsInitialized);
            Assert.IsFalse(vm.IsActive);
        }

        [TestMethod]
        public void AttachViewCallsViewLoadedAndViewReady()
        {
            PlatformProvider.Instance = provider;

            object loaded = null;
            object ready = null;

            TestViewModel vm = new TestViewModel();
            vm.Activate();

            vm.ViewLoadedPublic.Subscribe(x => loaded = x);
            vm.ViewReadyPublic.Subscribe(x => ready = x);

            object view = new object();
            vm.AttachView(view);
            
            Assert.IsNotNull(loaded);
            Assert.IsNotNull(ready);
        }
    }

    class TestViewModel : ReactiveViewModel
    {
        public TestViewModel(bool cacheView = true)
            : base(cacheView)
        {

        }

        public void DisableCacheView()
        {
            this.CacheView = false;
        }

        public IObservable<object> ViewLoadedPublic
        {
            get { return this.ViewLoaded; }
        }

        public IObservable<object> ViewReadyPublic
        {
            get { return this.ViewReady; }
        }
    }
}
