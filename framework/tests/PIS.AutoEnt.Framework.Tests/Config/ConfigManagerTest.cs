using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;
using PIS.AutoEnt.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.Config
{
    [TestClass]
    public class ConfigManagerTest
    {
        [TestInitialize]
        public void Setup()
        {
            ConfigurationManager.RefreshSection("PIS");
        }

        [TestMethod]
        [TestCategory("PIS.AutoEnt.Config")]
        public void Framework_Config_AppSettings_UploadFolder()
        {
            string uploadFolder = ConfigManager.AppSettings["UploadFolder"];

            Console.WriteLine("ConfigManager.AppSettings[\"UploadFolder\"]: " + uploadFolder);

            Assert.IsTrue(!String.IsNullOrEmpty(uploadFolder));
        }

        [TestMethod]
        [TestCategory("PIS.AutoEnt.Config")]
        public void Framework_Config_ServiceConfig_UserSession()
        {
            int? prepTimeOut = ConfigManager.ServiceConfig.UserSession.Get<int>("PrepTimeOut", 30);
            Console.WriteLine("ConfigManager.ServiceConfig.UserSession.Get<int>(\"PrepTimeOut\", 30): " + prepTimeOut);

            int? scanInterval = ConfigManager.ServiceConfig.UserSession.Get<int>("ScanInterval", 30);
            Console.WriteLine("ConfigManager.ServiceConfig.UserSession.Get<int>(\"ScanInterval\", 30): " + scanInterval);

            Assert.AreEqual(prepTimeOut, 30);
            Assert.AreEqual(scanInterval, 100);
        }

        [TestMethod]
        [TestCategory("PIS.AutoEnt.Config")]
        public void Framework_Config_EncryptConfig_MACAddress()
        {
            XmlNode xmlNode = ConfigManager.SystemConfig.GetConfig();

            // string maccode = "ABCDEFGH";
            string maccode = "78:E4:00:01:D2:15";

            XmlNode encryptNode = ConfigHelper.EncryptConfig(xmlNode, maccode);

            Console.WriteLine("EncryptConfig_MACAddress: " + encryptNode.OuterXml);
        }

        [TestMethod]
        [TestCategory("PIS.AutoEnt.Config")]
        public void Framework_Config_DecryptConfig_MACAddress()
        {
            XmlNode xmlNode = ConfigManager.SystemConfig.GetConfig();

            Console.WriteLine("DecryptConfig_MACAddress: " + xmlNode.OuterXml);
        }
    }
}
