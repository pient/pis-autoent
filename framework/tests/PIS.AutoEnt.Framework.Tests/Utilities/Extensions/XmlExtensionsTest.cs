using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PIS.AutoEnt.Tests.Utilities.Extensions
{
    [TestClass]
    public class XmlExtensionsTest
    {
        [TestMethod]
        public void Framework_XmlExtensions_LoadAsXmlDocument()
        {
            XmlDocument xmlDoc = null;

            xmlDoc = "<node><item /><item /></node>".LoadAsXmlDocument();
            Assert.IsNotNull(xmlDoc);
            Assert.AreEqual(xmlDoc.FirstChild.ChildNodes.Count, 2);

            xmlDoc = "<node attr1=\"x\" attr2=\"y\" attr3=\"z\"><item /><item /></node>".LoadAsXmlDocument();
            XmlDocument xmlDoc2 = "<node />".LoadAsXmlDocument();

            xmlDoc.DocumentElement.CopyAttributesTo(xmlDoc2.DocumentElement);
            Assert.AreEqual(xmlDoc2.DocumentElement.Attributes[0].Name, "attr1");
            Assert.AreEqual(xmlDoc2.DocumentElement.Attributes[0].Value, "x");
            Assert.AreEqual(xmlDoc2.DocumentElement.Attributes[1].Value, "y");
            Assert.AreEqual(xmlDoc2.DocumentElement.Attributes[2].Value, "z");

            string rtn = "<node><item /><item /></node>".GetFormatXml();
            rtn = xmlDoc.GetFormatXml();
            rtn = xmlDoc2.GetFormatXml();
            rtn = xmlDoc2.GetFormatXml();
        }
    }
}
