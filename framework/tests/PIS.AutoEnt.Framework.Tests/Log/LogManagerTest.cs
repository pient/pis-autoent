using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.Modules.Log
{
    [TestClass]
    public class LogManagerTest
    {
        [TestInitialize]
        public void Setup()
        {
            Sys.AppInitializer.Initialize();
        }

        [TestMethod]
        public void Framework_LogManager_GetLogger()
        {
            ILog logger = LogManager.GetLogger("Default");
            Assert.IsNotNull(logger);

            logger.Info("Default logging 1");

            LogManager.Log("Default logging 2");
            LogManager.Log("Default logging 2", new Exception("Default logging 2 Exception"));
        }
    }
}
