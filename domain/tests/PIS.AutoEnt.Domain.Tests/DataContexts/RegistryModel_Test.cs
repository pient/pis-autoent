using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Data;
using PIS.AutoEnt.DataContext;

namespace PIS.AutoEnt.Domain.Tests.DataContexts
{
    [TestClass]
    public class RegistryModel_Test
    {
        [TestMethod]
        public void RegistryModel_Serialize_Test()
        {
            RegisterNode node = new RegisterNode()
            {
            };

            node.Items.Add(new RegisterItem()
            {
            });

            var reg = new RegistryObject(node);

            var regxml = reg.ToXmlString();

            var reg2 = new RegistryObject(regxml);

            Assert.AreEqual(reg.Registry.Id, reg2.Registry.Id);
        }
    }
}
