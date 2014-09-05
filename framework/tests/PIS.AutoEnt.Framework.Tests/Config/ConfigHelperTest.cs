using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.Config;

namespace PIS.AutoEnt.Tests.Config
{
    [TestClass]
    public class ConfigHelperTest
    {
        [TestMethod]
        [TestCategory("PIS.AutoEnt.Config")]
        public void Framework_ConfigHelper_IsProtectedConfig()
        {
            XmlNode xmlNode = ConfigManager.SystemConfig.GetConfig();

            bool isProtected = ConfigHelper.IsProtectedConfig(xmlNode);

            Assert.IsFalse(isProtected);
        }
    }
}
