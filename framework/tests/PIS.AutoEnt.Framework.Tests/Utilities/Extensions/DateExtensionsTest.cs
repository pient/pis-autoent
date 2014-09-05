using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.Utilities.Extensions
{
    [TestClass]
    public class DateExtensionsTest
    {
        [TestMethod]
        public void Framework_DateExtensions_GetWeekOfYear()
        {
            Assert.AreEqual(new DateTime(2012, 1, 1).GetWeekOfYear(), 1);
            Assert.AreEqual(new DateTime(2012, 1, 7).GetWeekOfYear(), 1);
            Assert.AreEqual(new DateTime(2012, 1, 8).GetWeekOfYear(), 2);
            Assert.AreEqual(new DateTime(2012, 1, 14).GetWeekOfYear(), 2);
            Assert.AreEqual(new DateTime(2012, 1, 15).GetWeekOfYear(), 3);
            Assert.AreEqual(new DateTime(2012, 1, 28).GetWeekOfYear(), 4);
            Assert.AreEqual(new DateTime(2012, 2, 1).GetWeekOfYear(), 5);
        }

        [TestMethod]
        public void Framework_DateExtensions_GetQuarterOfYear()
        {
            Assert.AreEqual(new DateTime(2012, 1, 1).GetQuarterOfYear(), 1);
            Assert.AreEqual(new DateTime(2012, 3, 31).GetQuarterOfYear(), 1);
            Assert.AreEqual(new DateTime(2012, 4, 1).GetQuarterOfYear(), 2);
            Assert.AreEqual(new DateTime(2012, 7, 1).GetQuarterOfYear(), 3);
            Assert.AreEqual(new DateTime(2012, 10, 1).GetQuarterOfYear(), 4);
            Assert.AreEqual(new DateTime(2012, 12, 1).GetQuarterOfYear(), 4);
            Assert.AreEqual(new DateTime(2012, 12, 31).GetQuarterOfYear(), 4);
        }
    }
}
