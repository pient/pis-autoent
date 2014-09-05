using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIS.AutoEnt.XData;

namespace PIS.AutoEnt.Framework.Tests.Common.Data.DataObject
{
    [TestClass]
    public class XNodeTest
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
            Provider.Modify("insert <registry><system><portal /></system></registry> into (/XData)[1]");

            metadata = new XDataMeta("39bfe4f5-9200-4522-a173-6de37e77ab82", "SysObject");
            metadata.RootPath = "/XData/registry/system/portal";

            Provider = new XSqlDataStorage(metadata);
        }

        [TestCleanup]
        public void Clear()
        {
            TestingInitializer.ClearSysTestObject();
        }

        [TestMethod]
        public void Domain_XNode_XItemTest()
        {
            XDataNode portalDataNode = new XDataNode(Provider);

            XNode portalNode = portalDataNode.GetNode("/");
            portalNode.SetOperatingPath("/portal");

            PortalMenu menu = new PortalMenu(portalNode.XmlDoc);

            portalNode.AppendChild(menu);

            portalNode.ResetOperatingPath();

            XItemNode itemNode = menu.CreateChildItemNode("node", "item", "node");
            itemNode.SetAttr("code", "Fav");
            itemNode.SetAttr("name", "Fav");
            itemNode.SetAttr("title", "收藏夹");

            XItem item = itemNode.CreateChildItem();
            item.SetAttrs("code", "Home", "name", "Home", "title", "主页");

            item = itemNode.CreateChildItem();
            item.SetAttrs("code", "Workbench", "name", "Workbench", "title", "工作台");

            item = itemNode.CreateChildItem();
            item.SetAttrs("code", "Msg", "name", "Msg", "title", "消息");

            item = itemNode.CreateChildItem();
            item.SetAttrs("code", "Task", "name", "Task", "title", "任务");

            item = itemNode.CreateChildItem();
            item.SetAttrs("code", "Plan", "name", "Plan", "title", "计划");

            itemNode = menu.CreateChildItemNode("node", "item", "node");
            itemNode.SetAttrs("code", "App", "name", "App", "title", "应用");

            item = itemNode.CreateChildItem();
            item.SetAttrs("code", "Dev", "name", "Dev", "title", "开发", "securityCode", "Dev", "securityLevel", "90");

            item = itemNode.CreateChildItem();
            item.SetAttrs("code", "Admin", "name", "Admin", "title", "系统管理", "securityCode", "Admin", "securityLevel", "100");

            portalDataNode.InsertEle("/", "menu", menu.InnerXml, NodePosition.First);
            portalDataNode.ReplaceEle("menu", "menu", "<titem />");
            portalDataNode.InsertEle("/", "menu", menu.InnerXml, NodePosition.First);
            portalDataNode.RemoveAll("menu");
        }

        #region Support Classes

        private class PortalMenu : XNode
        {
            #region 构造函数

            public PortalMenu()
                : base()
            {

            }

            public PortalMenu(XmlDocument xmlDoc)
                : base(xmlDoc, "menu")
            {

            }

            #endregion
        }

        private class PortalMenuItem : XItem
        {
            #region 构造函数

            public PortalMenuItem(XmlDocument xmlDoc)
                : base(xmlDoc, "item")
            {

            }

            #endregion
        }

        private class PortalMenuItemList : XItemNode
        {
            #region 构造函数

            public PortalMenuItemList(XmlDocument xmlDoc)
                : base(xmlDoc, "item", "node")
            {

            }

            #endregion
        }

        #endregion
    }
}
