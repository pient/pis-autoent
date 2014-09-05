using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt.Tests.Data.DataObject
{
    [TestClass]
    public class XTagTest
    {
        private string TestXTagString_1 = "<Results GA0=\"0\" GA1=\"1\" GA2=\"2\"><TA TA3=\"T3\"><TA4>4</TA4></TA><CA>4</CA><CGP>2</CGP></Results>";

        [TestInitialize]
        public void Setup()
        {
        }

        [TestMethod]
        public void Framework_XTag_SetValueTest()
        {
            XTag tag = new XTag(TestXTagString_1);

            tag.SetValue(".", "GA0", "T1");
            string val = tag.GetValue(".", "GA0");
            Assert.AreEqual(val, "T1");

            tag.SetValue(".", "GA1", "T2");
            val = tag.GetValue(".", "GA1");
            Assert.AreEqual(val, "T2");

            tag.SetValue("./@GA2", "T3");
            val = tag.GetValue(".", "GA2");
            Assert.AreEqual(val, "T3");

            tag.SetValue("./CGP", "TGP");
            val = tag.GetValue("./CGP");
            Assert.AreEqual(val, "TGP");
        }

        [TestMethod]
        public void Framework_XTag_GetValueTest()
        {
            XTag tag = new XTag(TestXTagString_1);

            string attrVal = tag.GetValue(".", "GA0");
            Assert.IsNotNull(attrVal);

            string str_val = tag.GetValue("./@GA0");
            Assert.IsTrue(!String.IsNullOrEmpty(str_val));

            str_val = tag.GetValue("./@GA0");
            Assert.IsTrue(!String.IsNullOrEmpty(str_val));

            try
            {
                tag.GetValue("./GA0");
                Assert.Fail();
            }catch{}

            str_val = tag.GetValue(".");
            Assert.IsNotNull(str_val);

            int int_val = tag.GetValue<int>("./@GA0");
            Assert.IsTrue(int_val == 0);

            int_val = tag.GetValue<int>("./@GA1");
            Assert.IsTrue(int_val == 1);

            int_val = tag.GetValue<int>(".", "GA1");
            Assert.IsTrue(int_val == 1);

            int_val = tag.GetValue<int>("./TA/TA4");
            Assert.IsTrue(int_val == 4);

            try
            {
                tag.GetValue<int>("./GA1");
                Assert.Fail();
            } catch { }
        }

        [TestMethod]
        public void Framework_XTag_RemoveTest()
        {
            XTag tag = new XTag(TestXTagString_1);
            Assert.IsNotNull(tag.GetValue(".", "GA0"));
            tag.Remove(".", "GA0");

            try
            {
                tag.GetValue(".", "GA0");
                Assert.Fail();
            }
            catch { }

            Assert.IsNotNull(tag.GetValue(".", "GA1"));
            tag.Remove(".", "GA1");

            try
            {
                tag.GetValue(".", "GA1");
                Assert.Fail();
            }
            catch { }

            Assert.IsNotNull(tag.GetValue("./TA/@TA3"));
            tag.Remove("./TA/@TA3");

            try
            {
                tag.GetValue("./TA/@TA3");
                Assert.Fail();
            }
            catch { }

            Assert.IsNotNull(tag.GetValue("./TA/TA4"));
            tag.Remove("./TA/TA4");

            try
            {
                tag.GetValue("./TA/TA4");
                Assert.Fail();
            }
            catch { }
        }

        [TestMethod]
        public void Framework_XTag_ExistsTest()
        {
            XTag tag = new XTag(TestXTagString_1);
            Assert.IsTrue(tag.Exists(".", "GA0"));
            Assert.IsTrue(tag.Exists(".", "GA1"));
            Assert.IsTrue(tag.Exists("./TA/@TA3"));

            Assert.IsFalse(tag.Exists("ResultsX", "GA0"));
            Assert.IsFalse(tag.Exists(".", "XGA1"));
            Assert.IsFalse(tag.Exists("./TA/@XTA3"));
        }

        [TestMethod]
        public void Framework_XTag_InsertTest()
        {
            XTag tag = new XTag(TestXTagString_1);
            Assert.IsFalse(tag.Exists(".", "GATF"));
            tag.InsertAttr(".", "GATF", "TF", NodePosition.First);
            Assert.AreEqual(tag.GetValue(".", "GATF"), "TF");

            Assert.IsFalse(tag.Exists(".", "GATL"));
            tag.InsertAttr(".", "GATL", "TL", NodePosition.Last);
            Assert.AreEqual(tag.GetValue(".", "GATL"), "TL");

            Assert.IsFalse(tag.Exists(".", "GATB"));
            tag.InsertAttr("./@GA1", "GATB", "TB", NodePosition.Before);
            Assert.AreEqual(tag.GetValue(".", "GATB"), "TB");

            Assert.IsFalse(tag.Exists(".", "GATA"));
            tag.InsertAttr("./@GA1", "GATA", "TA", NodePosition.After);
            Assert.AreEqual(tag.GetValue(".", "GATA"), "TA");

            Assert.IsFalse(tag.Exists("./NTF"));
            tag.InsertEle(".", "NTF", "XXXXTF", NodePosition.First);
            Assert.AreEqual(tag.GetValue("./NTF"), "XXXXTF");

            Assert.IsFalse(tag.Exists("./NTL"));
            tag.InsertEle(".", "NTL", "XXXXTL", NodePosition.Last);
            Assert.AreEqual(tag.GetValue("./NTL"), "XXXXTL");

            Assert.IsFalse(tag.Exists("./NTB"));
            tag.InsertEle("./CA", "NTB", "XXXXTB", NodePosition.Before);
            Assert.AreEqual(tag.GetValue("./NTB"), "XXXXTB");

            Assert.IsFalse(tag.Exists("./NTA"));
            tag.InsertEle("./CA", "NTA", "XXXXTA", NodePosition.After);
            Assert.AreEqual(tag.GetValue("./NTA"), "XXXXTA");

            Assert.IsTrue(tag.Exists("./TA"));
            tag.ReplaceEle("./TA", "TT", "XXXXTT");
            Assert.IsFalse(tag.Exists("./TA"));
            Assert.AreEqual(tag.GetValue("./TT"), "XXXXTT");

        }
    }
}
