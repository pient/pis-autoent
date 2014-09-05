using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace PIS.AutoEnt.XData
{
    public class XNodeStore : IXObjectStorage
    {
        #region 成员属性

        public XmlDocument XmlDoc
        {
            get;
            set;
        }

        public XmlElement XmlEle
        {
            get;
            set;
        }

        #endregion

        #region 构造函数

        public XNodeStore()
        {
        }

        public XNodeStore(XmlElement xmlEle)
        {
            if (xmlEle != null)
            {
                this.XmlEle = xmlEle;
                this.XmlDoc = this.XmlEle.OwnerDocument;
            }
        }

        #endregion

        #region  XNodeStore 成员

        public XNode GetRoot()
        {
            return new XNode(this.XmlEle);
        }

        public bool Exists(string xpath)
        {
            return XmlEle.SelectSingleNode(xpath) != null;
        }

        public string GetValue(string xpath)
        {
            XmlNode node = XmlEle.SelectSingleNode(xpath);

            if (node == null)
            {
                throw new PISDataException(String.Format("xpath {0} doesn't exist", xpath));
            }
            else
            {
                return XObjectHelper.GetNodeValue(node);
            }
        }

        public void SetValue(string xpath, string value)
        {
            XmlNode node = this.PrepareNode(xpath);

            if (node is XmlAttribute)
            {
                node.Value = value;
            }
            else
            {
                node.InnerXml = value;
            }
        }

        public void Remove(string xpath)
        {
            XmlNode node = this.XmlEle.SelectSingleNode(xpath);

            if (node is XmlAttribute)
            {
                string p_xpath = GetParentPath(xpath);
                XmlElement ele = this.XmlEle.SelectSingleNode(p_xpath) as XmlElement;

                ele.RemoveAttribute(node.Name);
            }
            else
            {
                node.ParentNode.RemoveChild(node);
            }
        }

        public XNode GetSingleNode(string xpath)
        {
            XmlNode xmlNode = XmlEle.SelectSingleNode(xpath);

            return xmlNode == null ? null : new XNode(xmlNode as XmlElement);
        }

        public XNodeList GetNodes(string xpath)
        {
            XmlNodeList xmlNodeList = XmlEle.SelectNodes(xpath);

            return xmlNodeList == null ? null : new XNodeList(xmlNodeList);
        }

        /// <summary>
        /// 插入元素, Insert("Results", "NGX", "TNX", NodePosition.First)
        /// </summary>
        /// <param name="refpath"></param>
        /// <param name="xmlsrc">节点的InnerXml</param>
        /// <param name="position"></param>
        /// <returns></returns>
        public XNode InsertEle(string refpath, string eleName, string xmlsrc, NodePosition position)
        {
            XmlNode refnode = XmlEle.SelectSingleNode(refpath);

            XmlElement ele = this.XmlDoc.CreateElement(eleName);
            ele.InnerXml = xmlsrc;

            switch (position)
            {
                case NodePosition.After:
                    refnode.ParentNode.InsertAfter(ele, refnode);
                    break;
                case NodePosition.Before:
                    refnode.ParentNode.InsertBefore(ele, refnode);
                    break;
                case NodePosition.First:
                    refnode.PrependChild(ele);
                    break;
                case NodePosition.Last:
                    refnode.AppendChild(ele);
                    break;
            }

            return ele == null ? null : new XNode(ele);
        }

        /// <summary>
        /// 插入属性, Insert("Results", "GX", "TX", NodePosition.First)
        /// </summary>
        /// <param name="refpath"></param>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        /// <param name="position"></param>
        /// <returns>返回包含添加的属性所在的XmlElement</returns>
        public XNode InsertAttr(string refpath, string attrName, string attrValue, NodePosition position)
        {
            XmlNode refnode = null;
            XmlAttribute refattr = null;

            XmlAttribute attr = XmlDoc.CreateAttribute(attrName);
            attr.Value = attrValue;

            switch (position)
            {
                case NodePosition.After:
                    refattr = XmlEle.SelectSingleNode(refpath) as XmlAttribute;
                    refpath = this.GetParentPath(refpath);
                    refnode = XmlEle.SelectSingleNode(refpath);

                    refnode.Attributes.InsertAfter(attr, refattr);
                    break;
                case NodePosition.Before:
                    refattr = XmlEle.SelectSingleNode(refpath) as XmlAttribute;
                    refpath = this.GetParentPath(refpath);
                    refnode = XmlEle.SelectSingleNode(refpath);

                    refnode.Attributes.InsertBefore(attr, refattr);
                    break;
                case NodePosition.First:
                    refnode = this.PrepareNode(refpath);
                    refnode.Attributes.Prepend(attr);
                    break;
                case NodePosition.Last:
                    refnode = this.PrepareNode(refpath);
                    refnode.Attributes.Append(attr);
                    break;
            }

            return refnode == null ? null : new XNode(refnode as XmlElement);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 准备xpath, 若xpath存在，这直接返回，否则先创建再返回
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNode PrepareNode(string xpath)
        {
            XmlNode node = this.XmlEle.SelectSingleNode(xpath);

            if (node == null)
            {
                node = this.CreateNode(xpath);
            }

            return node;
        }

        /// <summary>
        /// 递归创建节点
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNode CreateNode(string xpath)
        {
            string[] xpathArray = xpath.Split('/');
            string nodePath = "";

            // 父节点初始化 
            XmlNode parentNode = this.XmlEle;

            // 逐层深入 XPath 各层级，如果结点不存在则创建 
            for (int i = 1; i < xpathArray.Length; i++)
            {
                XmlNode node = this.XmlEle.SelectSingleNode(nodePath + "/" + xpathArray[i]);

                if (node == null)
                {
                    if (xpathArray[i].IndexOf("@") == 0)
                    {
                        XmlAttribute newAttr = this.XmlDoc.CreateAttribute(xpathArray[i].TrimStart('@'));
                        node = parentNode.Attributes.Append(newAttr);
                    }
                    else
                    {
                        XmlElement newElement = this.XmlDoc.CreateElement(xpathArray[i]); // 创建结点
 
                        if (parentNode is XmlDocument)
                        {
                            node = this.XmlDoc.DocumentElement.AppendChild(newElement);
                        }
                        else
                        {
                            node = parentNode.AppendChild(newElement);
                        }
                    }
                }

                parentNode = node;
            }

            return parentNode;
        }

        /// <summary>
        /// 获取xpath的父节点
        /// </summary>
        /// <returns></returns>
        public string GetParentPath(string xpath)
        {
            xpath = xpath.TrimEnd('/');

            string p_xpath = xpath;

            if (IsAttrPath(xpath))
            {
                p_xpath = xpath.Substring(0, xpath.LastIndexOf("@") - 1);
            }
            else if (xpath.LastIndexOf("/") > 0)
            {
                p_xpath = xpath.Substring(0, xpath.LastIndexOf("/"));
            }

            return p_xpath;
        }

        /// <summary>
        /// 是否属性路径
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        private bool IsAttrPath(string xpath)
        {
            if (xpath.IndexOf("@") >= 0)
            {
                return true;
            }

            if (new Regex(@"@\w+$").IsMatch(xpath))
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
