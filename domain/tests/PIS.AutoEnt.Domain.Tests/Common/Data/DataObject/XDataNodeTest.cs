using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt.Framework.Tests.Common.Data.DataObject
{
    [TestClass]
    public class XDataNodeTest
    {
        private string TestXDataString_1 = "<Results GA0=\"0\" GA1=\"1\" GA2=\"2\"><TA TA3=\"T3\"><TA4>4</TA4></TA><CA>4</CA><CGP>2</CGP></Results>";

        XDataMeta metadata = null;
        XSqlDataStorage Store = null;

        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();

            TestingInitializer.InitializeSysTestObject();

            metadata = new XDataMeta(TestingInitializer.TestObjectId, "SysObject");
            metadata.RootPath = "/XData/registry/system/setting/test/Results";

            Store = new XSqlDataStorage(metadata);

            Store.CleanXData();  // 清空XData
            Store.PrepareXData();    // 准备XData

            Store.Modify("insert <registry><system /><app /><biz /></registry> into (/XData)[1]");

            // Prepare for testing
            Store.Modify("insert <setting><test>" + TestXDataString_1 + "</test></setting> into (/XData/registry/system)[1]");
        }

        [TestCleanup]
        public void Clear()
        {
            TestingInitializer.ClearSysTestObject();
        }

        [TestMethod]
        public void Domain_XDataNode_SetValueTest()
        {
            XDataNode node = new XDataNode(Store);

            node.SetValue(".", "GA0", "T1");
            string val = node.GetValue(".", "GA0");
            Assert.AreEqual(val, "T1");

            node.SetValue(".", "GA1", "T2");
            val = node.GetValue(".", "GA1");
            Assert.AreEqual(val, "T2");

            node.SetValue("./@GA2", "T3");
            val = node.GetValue(".", "GA2");
            Assert.AreEqual(val, "T3");

            node.SetValue("./CGP", "TGP");
            val = node.GetValue("./CGP");
            Assert.AreEqual(val, "TGP");
        }

        [TestMethod]
        public void Domain_XDataNode_GetValueTest()
        {
            XDataNode node = new XDataNode(Store);

            string attrVal = node.GetValue(".", "GA0");
            Assert.IsNotNull(attrVal);

            string str_val = node.GetValue("./@GA0");
            Assert.IsNotNull(str_val);

            str_val = node.GetValue("./@GA0");
            Assert.IsNotNull(str_val);

            // Assert.Catch(() => { node.GetValue("/Results/GA0"); });

            str_val = node.GetValue(".");
            Assert.IsNotNull(str_val);

            int int_val = node.GetValue<int>("./@GA0");
            Assert.IsTrue(int_val == 0);

            int_val = node.GetValue<int>("./@GA1");
            Assert.IsTrue(int_val == 1);

            int_val = node.GetValue<int>(".", "GA1");
            Assert.IsTrue(int_val == 1);

            int_val = node.GetValue<int>("./TA/TA4");
            Assert.IsTrue(int_val == 4);

            // Assert.Catch(() => { node.GetValue<int>("/Results/GA1"); });
        }

        [TestMethod]
        public void Domain_XDataNode_RemoveTest()
        {
            XDataNode node = new XDataNode(Store);
            Assert.IsNotNull(node.GetValue(".", "GA0"));
            node.Remove(".", "GA0");
            Assert.IsNull(node.GetValue(".", "GA0"));

            Assert.IsNotNull(node.GetValue(".", "GA1"));
            node.Remove(".", "GA1");
            Assert.IsNull(node.GetValue(".", "GA1"));

            Assert.IsNotNull(node.GetValue("./TA/@TA3"));
            node.Remove("./TA/@TA3");
            Assert.IsNull(node.GetValue("./TA/@TA3"));

            Assert.IsNotNull(node.GetValue("./TA/TA4"));
            node.Remove("./TA/TA4");
            Assert.IsNull(node.GetValue("./TA/TA4"));
        }

        [TestMethod]
        public void Domain_XDataNode_ExistsTest()
        {
            XDataNode node = new XDataNode(Store);

            Assert.IsTrue(node.Exists(".", "GA0"));
            Assert.IsTrue(node.Exists(".", "GA1"));
            Assert.IsTrue(node.Exists("./TA/@TA3"));

            Assert.IsFalse(node.Exists("ResultsX", "GA0"));
            Assert.IsFalse(node.Exists(".", "XGA1"));
            Assert.IsFalse(node.Exists("./TA/@XTA3"));
        }

        [TestMethod]
        public void Domain_XDataNode_InsertTest()
        {
            XDataNode node = new XDataNode(Store);
            Assert.IsFalse(node.Exists(".", "GATF"));
            node.InsertAttr(".", "GATF", "TF", NodePosition.First);
            Assert.AreEqual(node.GetValue(".", "GATF"), "TF");

            Assert.IsFalse(node.Exists(".", "GATL"));
            node.InsertAttr(".", "GATL", "TL", NodePosition.Last);
            Assert.AreEqual(node.GetValue(".", "GATL"), "TL");

            //Assert.IsFalse(node.Exists(".", "GATB"));
            //node.InsertAttr("./@GA1", "GATB", "TB", NodePosition.Before);
            //Assert.AreEqual(node.GetValue(".", "GATB"), "TB");

            //Assert.IsFalse(node.Exists(".", "GATA"));
            //node.InsertAttr("./@GA1", "GATA", "TA", NodePosition.After);
            //Assert.AreEqual(node.GetValue(".", "GATA"), "TA");

            Assert.IsFalse(node.Exists("./NTF"));
            node.InsertEle(".", "NTF", "XXXXTF", NodePosition.First);
            Assert.AreEqual(node.GetValue("./NTF"), "XXXXTF");

            Assert.IsFalse(node.Exists("./NTL"));
            node.InsertEle(".", "NTL", "XXXXTL", NodePosition.Last);
            Assert.AreEqual(node.GetValue("./NTL"), "XXXXTL");

            Assert.IsFalse(node.Exists("./NTB"));
            node.InsertEle("./CA", "NTB", "XXXXTB", NodePosition.Before);
            Assert.AreEqual(node.GetValue("./NTB"), "XXXXTB");

            Assert.IsFalse(node.Exists("./NTA"));
            node.InsertEle("./CA", "NTA", "XXXXTA", NodePosition.After);
            Assert.AreEqual(node.GetValue("./NTA"), "XXXXTA");

            Assert.IsTrue(node.Exists("./TA"));
            node.ReplaceEle("./TA", "TT", "XXXXTT");
            Assert.IsFalse(node.Exists("./TA"));
            Assert.AreEqual(node.GetValue("./TT"), "XXXXTT");

        }
    }
}
