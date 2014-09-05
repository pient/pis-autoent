using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.XData.DataStore;

namespace PIS.AutoEnt.Domain.Tests.Common.XData
{
    [TestClass]
    public class XDatastore_Test
    {
        private static XDataStore ds;

        [TestInitialize]
        public void Setup()
        {
            ds = new XDataStore();
        }

        [TestMethod]
        public void Initialize_Test()
        {
            //var region = new XDataRegion()
            //{
            //    Name = "Registry"
            //};

            //ds.Load(region);
        }
    }
}
