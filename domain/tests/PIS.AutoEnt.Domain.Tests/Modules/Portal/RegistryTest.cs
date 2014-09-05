using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.App;
using PIS.AutoEnt.XData.DataStore;

namespace PIS.AutoEnt.Domain.Tests.Modules.Portal
{
    [TestClass]
    public class RegistryTest
    {
        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();
        }

        [TestMethod]
        public void Domain_RegistryTest_RegDataStore()
        {
            App.AppInitializer.InitializeData();

            AppPortal.RegDataStore.PersistSysRegion();
        }

        [TestMethod]
        public void Domain_RegistryTest_RegDataSet()
        {
            var node = new RegDataNode();
            string regxml = CLRHelper.SerializeToXmlString(node);

            var ds = new RegDataSet();

            ds.Nodes.Add(
                new RegDataNode()
                {
                    Code = "Code",
                    Data = new object()
                });

            var dsxml = CLRHelper.SerializeToXmlString(ds); ;
        }
    }
}
