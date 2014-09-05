using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt.Framework.Tests.Common.Data.DataObject.Providers
{
    [TestClass]
    public class XSQLDataProviderTest
    {
        XDataMeta metadata = null;
        XSqlDataStorage Provider = null;

        [TestInitialize]
        public void Setup()
        {
            App.ModuleInitializer.Initialize();

            TestingInitializer.InitializeSysTestObject();

            metadata = new XDataMeta(TestingInitializer.TestObjectId, "SysObject", SysConsts.DefaultIdField, "/XData/registry/system/setting/test");

            Provider = new XSqlDataStorage(metadata);

            Provider.CleanXData();  // 清空XData
            Provider.PrepareXData();    // 准备XData
            Provider.Modify("insert <registry><system /><app /><biz /></registry> into (/XData)[1]");

            // Prepare for testing
            Provider.Modify("delete /XData/registry/system/setting/test");
            Provider.Modify("insert <setting><test string=\"a\" int=\"10\"><test1>abc</test1><test2>abc</test2></test></setting> into (/XData/registry/system)[1]");
        }

        [TestCleanup]
        public void Clear()
        {
            TestingInitializer.ClearSysTestObject();
        }

        [TestMethod]
        public void Domain_XSQLDataProvider_ExistsTest()
        {
            bool existsFlag = Provider.ExistsRoot();
        }

        [TestMethod]
        public void Domain_XSQLDataProvider_ModifyTest()
        {
            Provider.Modify("insert <values /> into (/XData/registry/system/setting/test)[1]");

            Provider.Modify("insert <string>Test 1</string> into (/XData/registry/system/setting/test/values)[1]");
            Provider.Modify("insert <int>100</int> into (/XData/registry/system/setting/test/values)[1]");
            Provider.Modify("insert <decimal>100.32</decimal> into (/XData/registry/system/setting/test/values)[1]");
            Provider.Modify("insert <bool>0</bool> into (/XData/registry/system/setting/test/values)[1]");
            Provider.Modify("insert <bool>1</bool> into (/XData/registry/system/setting/test/values)[1]");
            Provider.Modify("insert <bool>2</bool> into (/XData/registry/system/setting/test/values)[1]");
            Provider.Modify("insert <datetime>2012-11-7</datetime> into (/XData/registry/system/setting/test/values)[1]");
            Provider.Modify("insert <guid>39bfe4f5-9200-4522-a173-6de37e77ab82</guid> into (/XData/registry/system/setting/test/values)[1]");
        }

        [TestMethod]
        public void Domain_XSQLDataProvider_ValueTest()
        {
            Domain_XSQLDataProvider_ModifyTest();

            Assert.AreEqual("Test 1", Provider.Value<string>("(/XData/registry/system/setting/test/values/string)[1]"));
            Assert.AreEqual(100, Provider.Value<int>("(/XData/registry/system/setting/test/values/int)[1]"));
            Assert.AreEqual(100.32, Provider.Value<double>("(/XData/registry/system/setting/test/values/decimal)[1]"));
            Assert.AreEqual(false, Provider.Value<bool>("(/XData/registry/system/setting/test/values/bool)[1]"));
            Assert.AreEqual(true, Provider.Value<bool>("(/XData/registry/system/setting/test/values/bool)[2]"));
            Assert.AreEqual(true, Provider.Value<bool>("(/XData/registry/system/setting/test/values/bool)[3]"));
            Assert.AreEqual(new DateTime(2012, 11, 7), Provider.Value<DateTime>("(/XData/registry/system/setting/test/values/datetime)[1]"));
            Assert.AreEqual(new Guid("39bfe4f5-9200-4522-a173-6de37e77ab82"), Provider.Value<Guid>("(/XData/registry/system/setting/test/values/guid)[1]"));
        }

        [TestMethod]
        public void Domain_XSQLDataProvider_QueryTest()
        {
            string xdata = Provider.Query("(/XData)[1]");
        }

        [TestMethod]
        public void Domain_XSQLDataProvider_GetNodeTest()
        {
            Domain_XSQLDataProvider_ModifyTest();

            Assert.AreEqual("Test 1", Provider.GetValue("/values/string"));
        }

        [TestMethod]
        public void Domain_XSQLDataProvider_GetSetValueTest()
        {
            string xdata = Provider.GetValue("/");
            Assert.IsTrue(xdata.Length > 10);

            xdata = Provider.GetValue("/@string");
            Assert.AreEqual("a", xdata);

            Provider.SetValue("/@string", "a1");
            xdata = Provider.GetValue("/@string");
            Assert.AreEqual("a1", xdata);

            Provider.SetValue("/", "<string>b</string>");
            xdata = Provider.GetValue("/string");
            Assert.AreEqual("b", xdata);
        }

        [TestMethod]
        public void Domain_XSQLDataProvider_RemoveTest()
        {
            Assert.IsNotNull(Provider.GetValue("/@string"));
            Provider.Remove("/@string");
            Assert.IsNull(Provider.GetValue("/@string"));

            Assert.IsNotNull(Provider.GetValue("/@int"));
            Provider.Remove("/@int");
            Assert.IsNull(Provider.GetValue("/@int"));

            Assert.IsNotNull(Provider.GetValue("/test1"));
            Provider.Remove("/test1");
            Assert.IsNull(Provider.GetValue("/test1"));

            Assert.IsNotNull(Provider.GetValue("/"));
            Provider.Remove("/");
            Assert.IsNull(Provider.GetValue("/"));
        }

        [TestMethod]
        public void Domain_XSQLDataProvider_InsertTest()
        {
            Assert.IsNull(Provider.GetValue("/@int_f"));
            Provider.InsertAttr("/", "int_f", "10f", PIS.AutoEnt.XData.NodePosition.First);
            Assert.AreEqual("10f", Provider.GetValue("/@int_f"));

            Assert.IsNull(Provider.GetValue("/@int_l"));
            Provider.InsertAttr("/", "int_l", "10l", PIS.AutoEnt.XData.NodePosition.Last);
            Assert.AreEqual("10l", Provider.GetValue("/@int_l"));

            //// doesn't work
            //Assert.IsFalse(Provider.Exists("/XData/registry/system/setting/test/@int_b"));
            //Provider.InsertAttr("/registry/system/setting/test/@int", "int_b", "10b", PIS.Data.XData.NodePosition.Before);
            //Assert.AreEqual("10b", Provider.GetValue("/registry/system/setting/test/@int_b"));

            //// doesn't work
            //Assert.IsFalse(Provider.Exists("/XData/registry/system/setting/test/@int_a"));
            //Provider.InsertAttr("/registry/system/setting/test/@int", "int_a", "10a", PIS.Data.XData.NodePosition.After);
            //Assert.AreEqual("10a", Provider.GetValue("/registry/system/setting/test/@int_a"));

            Assert.IsNull(Provider.GetValue("/int_f"));
            Provider.InsertEle("/", "int_f", "10f", PIS.AutoEnt.XData.NodePosition.First);
            Assert.AreEqual("10f", Provider.GetValue("/int_f"));

            Assert.IsNull(Provider.GetValue("/int_l"));
            Provider.InsertEle("/", "int_l", "10l", PIS.AutoEnt.XData.NodePosition.Last);
            Assert.AreEqual("10l", Provider.GetValue("/int_l"));

            Assert.IsNull(Provider.GetValue("/int_b"));
            Provider.InsertEle("/int_f", "int_b", "10b", PIS.AutoEnt.XData.NodePosition.Before);
            Assert.AreEqual("10b", Provider.GetValue("/int_b"));

            Assert.IsNull(Provider.GetValue("/int_a"));
            Provider.InsertEle("/int_l", "int_a", "10a", PIS.AutoEnt.XData.NodePosition.After);
            Assert.AreEqual("10a", Provider.GetValue("/int_a"));
        }
    }
}
