using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using NUnit.Framework;

namespace PIS.Framework.Tests.Modules.Log
{
    [TestFixture]
    public class LogManagerTest
    {
        [SetUp]
        public void Setup()
        {
            AppSystem.Initialize();
        }

        [Test]
        public void GetLogger()
        {
            ILog logger = LogManager.GetLogger("DefLogger");
            Assert.IsNotNull(logger);

            logger.Info("Default logging 1");

            LogManager.Log("Default logging 2");
            LogManager.Log("Default logging 2", new Exception("Default logging 2 Exception"));
        }
    }
}
