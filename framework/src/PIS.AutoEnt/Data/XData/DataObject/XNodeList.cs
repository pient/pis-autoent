using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.XData
{
    public class XNodeList : XItemList<XNode>
    {
        #region 构造函数

        public XNodeList()
        {
            
        }

        public XNodeList(string xmlsrc)
        {
            XNode t_node = new XNode(xmlsrc);

            this.innerNodes = t_node.GetChildNodes().ToList();
        }

        public XNodeList(IEnumerable<XNode> nodes)
        {
            this.innerNodes = nodes.ToList();
        }

        /// <summary>
        /// XNodeList必须是XmlElement的集合
        /// </summary>
        /// <param name="nodeList"></param>
        public XNodeList(XmlNodeList nodeList)
        {
            this.innerNodes = new List<XNode>();

            foreach (XmlNode node in nodeList)
            {
                this.innerNodes.Add(new XNode(node as XmlElement));
            }
        }

        #endregion
    }
}
