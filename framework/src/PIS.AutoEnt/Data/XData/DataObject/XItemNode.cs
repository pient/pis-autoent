using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.XData
{
    /// <summary>
    /// 只包含XItem元素或ItemNode，只能由XNode创建
    /// </summary>
    public class XItemNode : XItem
    {
        #region 成员属性

        public override string RootName
        {
            get { return this.ItemNodeName; }
        }

        private string _ItemName = DefaultXItemName;
        private string _ItemNodeName = DefaultXItemNodeName;

        public virtual string ItemName
        {
            get { return _ItemName; }
        }

        public virtual string ItemNodeName
        {
            get { return _ItemNodeName; }
        }

        #endregion

        #region 构造函数

        internal XItemNode(string itemName = DefaultXItemName, string itemNodeName = DefaultXItemNodeName)
            : base(StringHelper.GetEmptyXmlNodeString(DefaultXItemNodeName))
        {
            this._ItemName = itemName;
            this._ItemNodeName = ItemNodeName;
        }

        public XItemNode(XmlDocument xmlDoc, string itemName = DefaultXItemName, string itemNodeName = DefaultXItemNodeName)
            : base(xmlDoc)
        {
            this._ItemName = itemName;
            this._ItemNodeName = ItemNodeName;
        }

        public XItemNode(XmlElement xmlEle, string itemName = DefaultXItemName, string itemNodeName = DefaultXItemNodeName)
            : base(xmlEle)
        {
            this._ItemName = itemName;
            this._ItemNodeName = ItemNodeName;
        }

        #endregion

        #region 成员函数

        public virtual bool HasChild()
        {
            return this.XmlEle.FirstChild != null;
        }

        public XItemList GetItems()
        {
            XmlNodeList nodeList = this.XmlEle.SelectNodes("//" + this.ItemName);

            return new XItemList(nodeList);
        }

        public XItemNodeList GetItemNodes()
        {
            XmlNodeList nodeList = this.XmlEle.SelectNodes("//" + this.ItemNodeName);

            return new XItemNodeList(nodeList);
        }

        public virtual XItem CreateChildItem()
        {
            XmlElement ele = this.XmlDoc.CreateElement(this.ItemName);
            this.XmlEle.AppendChild(ele);

            XItem item = new XItem(ele);

            return item;
        }

        public virtual XItemNode CreateChildItemNode(string itemName = DefaultXItemName, string itemNodeName = DefaultXItemNodeName)
        {
            XmlElement ele = this.XmlDoc.CreateElement(this.ItemNodeName);
            this.XmlEle.AppendChild(ele);

            XItemNode node = new XItemNode(ele, itemName, itemNodeName);

            return node;
        }


        #endregion
    }
}
