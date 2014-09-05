using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using PIS.AutoEnt;

namespace PIS.AutoEnt.XData
{
    /// <summary>
    /// XData中最小元素，只有属性没有InnerXml，只能由XNode创建
    /// </summary>
    [Serializable]
    public class XItem : XObject
    {
        #region 成员属性

        public const string DefaultXItemName = "XItem";

        public const string DefaultXItemNodeName = "XItemNode";

        private string _RootName = DefaultXItemName;

        public override string RootName
        {
            get { return _RootName; }
        }

        public XmlDocument XmlDoc
        {
            get;
            private set;
        }

        public XmlElement XmlEle
        {
            get;
            protected set;
        }

        #endregion

        #region 构造函数

        internal XItem()
            : this(StringHelper.GetEmptyXmlNodeString(DefaultXItemName))
        {
        }

        internal XItem(string xmlsrc)
            : base()
        {
            this.XmlDoc = new XmlDocument();
            this.XmlDoc.LoadXml(xmlsrc);
            this.XmlEle = this.XmlDoc.DocumentElement;

            this.Initialize(this.XmlEle);
        }

        public XItem(XmlDocument xmlDoc)
            : this(xmlDoc, DefaultXItemName)
        {
        }

        public XItem(XmlDocument xmlDoc, string name)
        {
            this.XmlDoc = xmlDoc;
            this.XmlEle = this.XmlDoc.CreateElement(name);

            this.Initialize(this.XmlEle);
        }

        public XItem(XmlElement xmlEle)
            : base()
        {
            this.Initialize(xmlEle);
        }

        protected virtual void Initialize(XmlElement xmlEle)
        {
            if (xmlEle != null)
            {
                this.XmlEle = xmlEle;
                this.XmlDoc = xmlEle.OwnerDocument;
                this._RootName = this.XmlEle.Name;
            }

            this.Store = new XNodeStore(this.XmlEle);
        }

        #endregion

        #region 成员函数

        public virtual XNode GetParent()
        {
            return new XNode(this.XmlEle.ParentNode as XmlElement);
        }

        public virtual XNode GetPreviousSibling()
        {
            return new XNode(this.XmlEle.PreviousSibling as XmlElement);
        }

        public virtual XNode GetNextSibling()
        {
            return new XNode(this.XmlEle.NextSibling as XmlElement);
        }

        public virtual void RemoveAll()
        {
            this.XmlEle.RemoveAll();
        }

        /// <summary>
        /// 删除符合Xpath条件的所有节点
        /// </summary>
        /// <param name="xpath"></param>
        public virtual void RemoveAll(string xpath)
        {
            XmlNodeList nodeList = this.XmlEle.SelectNodes(xpath);

            foreach (XmlNode node in nodeList)
            {
                this.XmlEle.RemoveChild(node);
            }
        }

        public virtual XNode Clone(bool includeChild)
        {
            XmlElement node = (XmlElement)this.XmlEle.CloneNode(includeChild);
            return new XNode(this.XmlEle);
        }

        /// <summary>
        /// 将本对象转换成为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.XmlEle.OuterXml;
        }

        /// <summary>
        /// 将本对象转换为JsonString
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public virtual string ToJsonString(string xpath = null)
        {
            XmlNode node = this.XmlEle;

            if (!String.IsNullOrEmpty(xpath))
            {
                node = this.XmlEle.SelectSingleNode(xpath);
            }

            return node.ToJsonString();
        }

        #endregion

        #region 属性操作

        /// <summary>
        /// 获取所有属性
        /// </summary>
        /// <returns></returns>
        public virtual EasyDictionary GetAttrs()
        {
            EasyDictionary attrs = new EasyDictionary();

            foreach (XmlAttribute attr in this.XmlEle.Attributes)
            {
                attrs.Set(attr.Name, attr.Value);
            }

            return attrs;
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="attrs"></param>
        public virtual void SetAttrs(EasyDictionary attrs)
        {
            foreach (string key in attrs.Keys)
            {
                this.SetAttr(key, attrs[key].ToString());
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="val"></param>
        public virtual void SetAttrs(params string[] keyVals)
        {
            if (keyVals.Length == 0)
            {
                throw new PISException(new ArgumentException("No key value provided."));
            }
            else if (0 != keyVals.Length % 2)
            {
                new PISException(new ArgumentException("Key values count don't match."));
            }

            for (int i = 0; i < keyVals.Length; )
            {
                this.SetAttr(keyVals[i], keyVals[(i + 1)]);
                i = i + 2;
            }
        }

        /// <summary>
        /// 根据index获得节点值
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual string GetAttr(int index)
        {
            string value = null;
            XmlNode node = this.XmlEle.Attributes.Item(index);
            if (node != null) value = node.Value ?? String.Empty;
            return value;
        }

        public virtual string GetAttr(string name)
        {
            return this.XmlEle.GetAttribute(name);
        }

        /// <summary>
        /// 根据index设置节点值
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public virtual void SetAttr(int index, string value)
        {
            XmlNode node = this.XmlEle.Attributes.Item(index);
            node.Value = value;
        }

        public virtual void SetAttr(string name, string value)
        {
            this.XmlEle.SetAttribute(name, value);
        }

        /// <summary>
        /// 根据index删除节点
        /// </summary>
        /// <param name="index"></param>
        public virtual void RemoveAttr(int index)
        {
            XmlNode node = this.XmlEle.Attributes.Item(index);
            this.XmlEle.Attributes.RemoveNamedItem(node.Name);
        }

        public virtual void RemoveAttr(string name)
        {
            this.XmlEle.RemoveAttribute(name);
        }

        public virtual bool HasAttr(string name)
        {
            return this.XmlEle.HasAttribute(name);
        }

        public virtual void RemoveAllAttr()
        {
            this.XmlEle.RemoveAllAttributes();
        }

        /// <summary>
        /// 根据index获得节点名称
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public virtual string GetAttrName(int index)
        {
            return this.XmlEle.Attributes.Item(index).Name;
        }

        /// <summary>
        /// 获得属性节点数量
        /// </summary>
        /// <returns></returns>
        public virtual int GetAttrCount()
        {
            return this.XmlEle.Attributes.Count;
        }

        #endregion
    }
}
