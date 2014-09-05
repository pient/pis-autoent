using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.DataAccess.System
{
    [TestClass]
    public class PortalRepositoryTest
    {
        [TestInitialize]
        public void Setup()
        {
            Sys.AppInitializer.Initialize();
        }

        [TestMethod]
        public void PortalRepository_GetMenu()
        {
            // IPortalRepository repository = ObjectManager.Resolve<IPortalRepository>();

            //SysPortalMenu menu = repository.GetMenu("f87ce89c-4054-4bac-93ca-a119016716e7");

            //Assert.IsNotNull(menu);
        }
    }
}
