using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using PIS.AutoEnt.Data;

namespace PIS.AutoEnt.XData
{
    public class XTag : XNode, ITag
    {
        public const string DefaultXTagName = "XTag";

        #region 成员属性

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造空的XTag
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        public XTag()
            : this(StringHelper.GetEmptyXmlNodeString(DefaultXTagName))
        {
        }

        /// <summary>
        /// 由字符串构造XTag
        /// </summary>
        /// <param name="xmlstr"></param>
        public XTag(string xmlstr)
            : base(xmlstr)
        {
        }

        /// <summary>
        /// 根据一个XmlDocument和一个XmlElement构造XTag
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="ele"></param>
        public XTag(XmlElement xmlEle)
            : base(xmlEle)
        {
        }

        #endregion
    }
}
