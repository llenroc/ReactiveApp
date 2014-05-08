using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReactiveUI;
using Splat;

namespace ReactiveApp.Test
{
    [TestClass]
    public class IMutableDependencyResolverExtensionsTests
    {
        [TestMethod]
        public void CanRegisterViewTypeForViewModelTypeTest()
        {
            ModernDependencyResolver locator = new ModernDependencyResolver();

            locator.RegisterView<ActivatingView, ActivatingViewModel>();

            Type type = locator.GetService(typeof(IViewFor<ActivatingViewModel>)) as Type;

            Assert.AreEqual(typeof(ActivatingView), type);
        }
    }
}
