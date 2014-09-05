using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.App;

namespace PIS.AutoEnt.Framework.Tests.Modules.Modules.System
{
    [TestClass]
    public class AppInitializer_Test
    {
        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
        }

        [TestMethod]
        public void Domain_Initialize_Test()
        {
            App.AppInitializer.InitializeData();
        }

        // [TestMethod]
        public void Domain_Rollback_Test()
        {
            App.AppInitializer.RollbackData();
        }
    }
}
