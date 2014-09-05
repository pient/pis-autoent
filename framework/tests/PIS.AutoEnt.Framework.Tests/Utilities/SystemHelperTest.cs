using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.Utilities
{
    [TestClass]
    public class SystemHelperTest
    {
        [TestMethod]
        public void Framework_NewCombId()
        {
            Guid guid = SystemHelper.NewCombId();

            Assert.AreNotEqual(guid, Guid.Empty);
        }

        [TestMethod]
        public void Framework_SystemHelper_GetPath_Current()
        {
            string path = SystemHelper.GetPath();

            Console.WriteLine("SystemHelper.GetPath(): " + path.TrimEnd('\\'));

            Assert.IsTrue(!String.IsNullOrEmpty(path));
            Assert.AreEqual(System.Environment.CurrentDirectory.TrimEnd('\\'), path.TrimEnd('\\'));
        }

        [TestMethod]
        public void Framework_SystemHelper_GetPath_Relative()
        {
            string path = SystemHelper.GetPath("..\\..\\pis.sys.config");

            Console.WriteLine("SystemHelper.GetPath(\"..\\..\\pis.sys.config\"): " + path);

            Assert.IsTrue(!String.IsNullOrEmpty(path));
            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Framework_SystemHelper_GetMACCode()
        {
            string maccode = SystemHelper.GetMACCode();

            Console.WriteLine("SystemHelper.GetMACCode(): " + maccode);

            Assert.IsTrue(!String.IsNullOrEmpty(maccode));
        }

        [TestMethod]
        public void Framework_SystemHelper_CombIdTest()
        {
            Guid combId = SystemHelper.NewCombId();

            DateTime dateTime = SystemHelper.GetDateFromCombId(combId);
        }
    }
}
