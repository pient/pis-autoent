using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.Utilities.Extensions
{
    [TestClass]
    public class EnumerableExtensionsTest
    {
        [TestMethod]
        public void Framework_EnumerableExtensions_BuildArray()
        {
            IList a = new List<object>();
            a.Add("a");
            a.Add(1);
            a.Add(1.12);
            a.Add(true);

            Array rtn = a.BuildArray<object>();
            Assert.IsFalse(rtn == a);
            Assert.AreEqual(rtn.Length, a.Count);
            Assert.AreEqual(rtn.GetValue(0), a[0]);
            Assert.AreEqual(rtn.GetValue(3), a[3]);
        }
    }
}
