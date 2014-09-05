using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt.Tests.Data.DataObject
{
    [TestClass]
    public class XNodeTest
    {
        private string TestXTagString_1 = "<XNode><Results GA0=\"0\" GA1=\"1\" GA2=\"2\"><TA TA3=\"T3\"><TA4>4</TA4></TA><CA>4</CA><CGP>2</CGP></Results></XNode>";

        [TestMethod]
        public void Framework_XNode_SetAttrTest()
        {
            XNode node = new XNode(TestXTagString_1);

            node.SetAttr("XX", "YY");
            Assert.AreEqual(node.GetAttr("XX"), "YY");
        }
    }
}
