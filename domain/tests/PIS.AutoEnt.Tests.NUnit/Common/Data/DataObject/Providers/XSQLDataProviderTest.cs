using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace PIS.Framework.Tests.Common.Data.DataObject.Providers
{
    public class XSQLDataProviderTest
    {
        XDataMetadata metadata = null;
        XSQLDataProvider Provider = null;

        [SetUp]
        public void Setup()
        {
            AppSystem.Initialize();

            metadata = new XDataMetadata("39bfe4f5-9200-4522-a173-6de37e77ab82", "SysObject");
            Provider = new XSQLDataProvider(metadata);

            Provider.CleanXData();  // 清空XData
            Provider.PrepareXData();    // 准备XData
            Provider.Modify("insert <registry><system /><app /><biz /></registry> into (/XData)[1]");
        }

        [Test]
        public void ExistsTest()
        {
            bool existsFlag = Provider.ExistsRoot();
        }

        [Test]
        public void ModifyTest()
        {
            Provider.Modify("delete /XData/registry/system/setting");
            Provider.Modify("insert <setting /> into (/XData/registry/system)[1]");
        }

        [Test]
        public void ValueTest()
        {

        }

        [Test]
        public void Nodes()
        {

        }

        [Test]
        public void QueryTest()
        {
            string xdata = Provider.Query("(/XData)[1]");
        }
    }
}
