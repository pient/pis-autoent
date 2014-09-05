using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Newtonsoft.Json.Serialization;
using PIS.AutoEnt;

namespace PIS.AutoEnt.XData
{
    public class XNode : XItem
    {
        public const string DefaultXNodeName = "XNode";

        #region 成员属性

        public string InnerXml
        {
            get { return this.XmlEle.InnerXml; }
            set { this.XmlEle.InnerXml = value; }
        }

        public string InnerText
        {
            get { return this.XmlEle.InnerText; }
            set { this.XmlEle.InnerText = value; }
        }

        public string OuterXml
        {
            get { return this.XmlEle.OuterXml; }
        }

        public string OperatingPath
        {
            get;
            protected set;
        }

        public string OperatingXPath
        {
            get
            {
                return this.GetOperatingXPath("");
            }
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造空的XTag
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        public XNode()
            : this(StringHelper.GetEmptyXmlNodeString(DefaultXNodeName))
        {
        }

        public XNode(string xmlsrc)
            : base(xmlsrc)
        {
        }

        public XNode(XmlDocument xmlDoc)
            : this(xmlDoc, DefaultXNodeName)
        {
        }

        public XNode(XmlDocument xmlDoc, string rootName)
            : base(xmlDoc, rootName)
        {
        }

        /// <summary>
        /// 根据一个XmlDocument和一个XmlElement构造XNode
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        public XNode(XmlElement ele)
            : base(ele)
        {
        }

        #endregion

        #region 成员函数

        /// <summary>
        /// 设置操作路径
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public string SetOperatingPath(string xpath)
        {
            this.OperatingPath = xpath;

            return this.OperatingPath;
        }

        public string ResetOperatingPath()
        {
            return this.SetOperatingPath("");
        }

        /// <summary>
        /// 创建XItem
        /// </summary>
        /// <param name="rootName"></param>
        /// <returns></returns>
        public virtual XItem CreateItem(string rootName)
        {
            XmlElement ele = this.XmlDoc.CreateElement(rootName);

            XItem item = new XItem(ele);

            return item;
        }

        /// <summary>
        /// 创建并附加XItem
        /// </summary>
        /// <param name="rootName"></param>
        /// <returns></returns>
        public virtual XItem CreateChildItem(string rootName)
        {
            XmlElement ele = this.XmlDoc.CreateElement(rootName);
            this.XmlEle.AppendChild(ele);

            XItem item = new XItem(ele);

            return item;
        }

        /// <summary>
        /// 创建XItemNode
        /// </summary>
        /// <param name="rootName"></param>
        /// <param name="itemName"></param>
        /// <param name="itemNodeName"></param>
        /// <returns></returns>
        public virtual XItemNode CreateItemNode(string rootName = DefaultXItemNodeName, string itemName = DefaultXItemName, string itemNodeName = DefaultXItemNodeName)
        {
            XmlElement ele = this.XmlDoc.CreateElement(rootName);

            XItemNode node = new XItemNode(ele, itemName, itemNodeName);

            return node;
        }

        /// <summary>
        /// 创建并附加XItemNode
        /// </summary>
        /// <param name="rootName"></param>
        /// <param name="itemName"></param>
        /// <param name="itemNodeName"></param>
        /// <returns></returns>
        public virtual XItemNode CreateChildItemNode(string rootName = DefaultXItemNodeName, string itemName = DefaultXItemName, string itemNodeName = DefaultXItemNodeName)
        {
            XmlElement ele = this.XmlDoc.CreateElement(rootName);
            this.XmlEle.AppendChild(ele);

            XItemNode node = new XItemNode(ele, itemName, itemNodeName);

            return node;
        }

        public virtual bool HasChild()
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                return node.FirstChild != null;
            }
            else
            {
                return false;
            }
        }

        public virtual XNode GetFirstChild()
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                return new XNode(node.FirstChild as XmlElement);
            }
            else
            {
                return null;
            }
        }

        public virtual XNode GetLastChild()
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                return new XNode(node.LastChild as XmlElement);
            }
            else
            {
                return null;
            }
        }

        public virtual XNode GetChildNode(string name)
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                return new XNode(node[name]);
            }
            else
            {
                return null;
            }
        }

        public virtual XNodeList GetChildNodes()
        {
            return this.GetNodeList(this.OperatingPath);
        }

        public virtual XNode PreappendChild(XItem newChild)
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                XmlNode tnode = node.PrependChild(newChild.XmlEle);

                return new XNode(tnode as XmlElement);
            }
            else
            {
                return null;
            }
        }

        public virtual XNode AppendChild(XItem newChild)
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                XmlNode tnode = node.AppendChild(newChild.XmlEle);

                return new XNode(tnode as XmlElement);
            }
            else
            {
                return null;
            }
        }

        public virtual XNode InsertAfter(XItem newNode, XItem refNode)
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                XmlNode tnode = node.InsertAfter(newNode.XmlEle, refNode.XmlEle);

                return new XNode(tnode as XmlElement);
            }
            else
            {
                return null;
            }
        }

        public virtual XNode InsertBefore(XItem newNode, XItem refNode)
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                XmlNode tnode = node.InsertBefore(newNode.XmlEle, refNode.XmlEle);

                return new XNode(tnode as XmlElement);
            }
            else
            {
                return null;
            }
        }

        public virtual XNode RemoveChild(XItem oldChild)
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                XmlNode tnode = node.RemoveChild(oldChild.XmlEle);

                return new XNode(tnode as XmlElement);
            }
            else
            {
                return null;
            }
        }

        public virtual XNode ReplaceChild(XItem newChild, XItem oldChild)
        {
            XmlNode node = this.XmlEle.SelectSingleNode(this.OperatingXPath);

            if (node != null)
            {
                XmlNode tnode = node.ReplaceChild(newChild.XmlEle, oldChild.XmlEle);

                return new XNode(tnode as XmlElement);
            }
            else
            {
                return null;
            }
        }

        public virtual string GetValue()
        {
            return this.GetValue(this.OperatingPath);
        }

        public virtual void SetValue(string value)
        {
            this.SetValue(this.OperatingPath, value);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 获取操作路径
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        protected string GetOperatingXPath(string xpath)
        {
            string op_xpath = xpath;

            if (!String.IsNullOrEmpty(this.OperatingPath))
            {
                xpath = xpath.TrimStart(xpath);

                if (String.IsNullOrEmpty(xpath))
                {
                    op_xpath = this.OperatingPath;
                }
                else
                {
                    op_xpath = this.OperatingPath + "/" + xpath;
                }
            }

            return this.GetXPath(op_xpath);
        }

        #endregion
    }
}
