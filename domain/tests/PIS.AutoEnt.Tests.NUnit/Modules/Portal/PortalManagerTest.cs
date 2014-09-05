using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Castle.Windsor;

namespace PIS.Framework.Tests.Modules.Portal
{
    [TestFixture]
    public class PortalManagerTest
    {
        [SetUp]
        public void Setup()
        {
            AppSystem.Initialize();
        }

        [Test]
        public void CurrentUser()
        {
            Assert.IsNotNull(PortalManager.CurrentUser);
        }
    }
}
