using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PIS.AutoEnt.Config
{
    public class ThirdPartyConfig : ConfigBase
    {
        #region 属性

        /// <summary>
        /// 服务节点
        /// </summary>
        public XmlNodeList ThirdPartyNodes
        {
            get { return configData.ChildNodes; }
        }

        #endregion

        #region 构造函数

        public ThirdPartyConfig(XmlNode sections)
            : base(sections)
        {
        }

        #endregion

        #region 私有函数

        #endregion
    }
}
