using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.Modules.Portal
{
    [TestClass]
    public class PortalManagerTest
    {
        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
        }

        [TestMethod]
        public void Domain_PortalManager_CurrentUser()
        {
            // Assert.IsNotNull(PortalManager.CurrentUser);
        }
    }
}
