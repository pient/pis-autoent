using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.XData
{
    public class XItemNodeList : XItemList<XItemNode>
    {
        #region 构造函数

        public XItemNodeList()
        {
        }

        public XItemNodeList(string xmlsrc)
        {
            XItemNode t_node = new XItemNode(xmlsrc);

            this.innerNodes = t_node.GetItemNodes().ToList();
        }

        public XItemNodeList(IEnumerable<XItemNode> nodes)
        {
            this.innerNodes = nodes.ToList();
        }

        /// <summary>
        /// XNodeList必须是XmlElement的集合
        /// </summary>
        /// <param name="nodeList"></param>
        public XItemNodeList(XmlNodeList itemList)
        {
            this.innerNodes = new List<XItemNode>();

            foreach (XmlNode item in itemList)
            {
                this.innerNodes.Add(new XItemNode(item as XmlElement));
            }
        }

        #endregion
    }
}
