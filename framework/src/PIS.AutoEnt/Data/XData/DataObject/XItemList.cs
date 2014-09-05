using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.XData
{
    public class XItemList : XItemList<XItem>
    {
        #region 构造函数

        public XItemList()
        {
        }

        public XItemList(string xmlsrc)
        {
            XItemNode t_node = new XItemNode(xmlsrc);

            this.innerNodes = t_node.GetItems().ToList();
        }

        public XItemList(IEnumerable<XItem> nodes)
        {
            this.innerNodes = nodes.ToList();
        }

        /// <summary>
        /// XNodeList必须是XmlElement的集合
        /// </summary>
        /// <param name="nodeList"></param>
        public XItemList(XmlNodeList itemList)
        {
            this.innerNodes = new List<XItem>();

            foreach (XmlNode item in itemList)
            {
                this.innerNodes.Add(new XItem(item as XmlElement));
            }
        }

        #endregion
    }
}
